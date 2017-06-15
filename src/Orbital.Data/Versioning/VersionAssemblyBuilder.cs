﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.EntityFrameworkCore.Metadata;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using MethodAttributes = Mono.Cecil.MethodAttributes;
using TypeAttributes = Mono.Cecil.TypeAttributes;

namespace Orbital.Data.Versioning
{
    internal class VersionAssemblyBuilder
    {
        private readonly AssemblyDefinition _assembly;
        private readonly ModuleDefinition _module;

        private readonly TypeReference _versionEntityGenericDefinition;

        private const string VersionEntityIdName = nameof(IVersionEntity<object>.Id);
        private const string VersionEntityDateName = nameof(IVersionEntity<object>.Date);
        private const string VersionEntityToEntityName = nameof(IVersionEntity<object>.ToEntity);

        private static readonly ConstructorInfo DefaultObjectConstructor = typeof(object).GetConstructor();
        private static readonly MethodInfo DateTimeUtcNowGetMethod = typeof(DateTime).GetRuntimeProperty(nameof(DateTime.UtcNow)).GetMethod;

        public VersionAssemblyBuilder(string assemblyName = "Autogen.Versioning")
        {
            _assembly = AssemblyDefinition.CreateAssembly(new AssemblyNameDefinition(assemblyName, new Version(1, 0)), assemblyName, ModuleKind.Dll);
            _module = _assembly.MainModule;

            _versionEntityGenericDefinition = _module.ImportReference(typeof(IVersionEntity<>));
        }

        public Assembly Build()
        {
            using (var memoryStream = new MemoryStream())
            {
                _assembly.Write(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return AssemblyLoadContext.Default.LoadFromStream(memoryStream);
            }
        }

        public VersionModel Add(IEntityType entityModel)
        {
            var entityType = _module.ImportReference(entityModel.ClrType);
            var versionType = _module.DefineType(entityType.Name + "Version", TypeAttributes.Class | TypeAttributes.Public, _module.TypeSystem.Object);

            // Implement IVersionEntity<TEntity>
            var versionEntity = new GenericInstanceType(_versionEntityGenericDefinition);
            versionEntity.GenericArguments.Add(entityType);
            versionType.Interfaces.Add(new InterfaceImplementation(versionEntity));

            // Id & Date columns
            var idColumn = DefineColumnProperty(versionType, VersionEntityIdName, _module.TypeSystem.Int64);
            var dateColumn = DefineColumnProperty(versionType, VersionEntityDateName, _module.ImportReference(typeof(DateTime)));

            // Copy all the non-navigation columns from the entity, prefix the with Field_ to avoid collisions with version columns
            var fieldMappings = new Dictionary<IProperty, PropertyDefinition>();
            foreach (var prop in entityModel.GetProperties())
            {
                var property = DefineColumnProperty(versionType, "Field_" + prop.Name, _module.ImportReference(prop.ClrType));
                fieldMappings.Add(prop, property);
            }

            var versionEntityType = new VersionModel(
                entityType, versionType,
                idColumn, dateColumn,
                entityModel, fieldMappings);

            // Define the default & entity constructors
            DefineConstructors(versionType, entityType, versionEntityType);
            // Finish implementing IVersionEntity<TEntity>
            DefineToEntity(versionType, entityType, versionEntityType);

            return versionEntityType;
        }

        private PropertyDefinition DefineColumnProperty(TypeDefinition type, string id, TypeReference propertyType)
        {
            var columnAttributeConstructor = _module.ImportReference(typeof(ColumnAttribute).GetConstructor(typeof(string)));
            var columnAttribute = new CustomAttribute(columnAttributeConstructor);
            columnAttribute.ConstructorArguments.Add(new CustomAttributeArgument(_module.TypeSystem.String, id));

            var property = type.DefineAutoProperty(id, propertyType);
            property.CustomAttributes.Add(columnAttribute);

            return property;
        }

        private void DefineConstructors(TypeDefinition type, TypeReference entityType, VersionModel versionModel)
        {
            const string name = ".ctor";
            const MethodAttributes attr = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;

            var defaultObjectConstructor = _module.ImportReference(DefaultObjectConstructor);

            // default constructor
            var defaultContructor = type.DefineMethod(name, attr, _module.TypeSystem.Void);
            var defaultConstructorIL = defaultContructor.Body.GetILProcessor();

            defaultConstructorIL.Emit(OpCodes.Ldarg_0);
            defaultConstructorIL.Emit(OpCodes.Call, defaultObjectConstructor);
            defaultConstructorIL.Emit(OpCodes.Ret);

            // from entity constructor
            var entityContructor = type.DefineMethod(name, attr, _module.TypeSystem.Void);
            var entity = new ParameterDefinition(entityType);
            entityContructor.Parameters.Add(entity);

            var entityContructorIL = entityContructor.Body.GetILProcessor();

            // base()
            entityContructorIL.Emit(OpCodes.Ldarg_0); // this
            entityContructorIL.Emit(OpCodes.Call, defaultObjectConstructor);

            // this.Date = DateTime.UtcNow;
            entityContructorIL.Emit(OpCodes.Ldarg_0); // this
            entityContructorIL.Emit(OpCodes.Call, _module.ImportReference(DateTimeUtcNowGetMethod));
            entityContructorIL.Emit(OpCodes.Call, versionModel.DateColumn.SetMethod);

            foreach (var fieldMapping in versionModel.EntityFieldMappings)
            {
                // this.A = entity.A;
                var entityField = fieldMapping.Key.PropertyInfo;
                var historyField = fieldMapping.Value;

                entityContructorIL.Emit(OpCodes.Ldarg_0); // this
                entityContructorIL.Emit(OpCodes.Ldarg, entity);
                entityContructorIL.Emit(OpCodes.Callvirt, _module.ImportReference(entityField.GetMethod));
                entityContructorIL.Emit(OpCodes.Call, historyField.SetMethod);
            }

            // return
            entityContructorIL.Emit(OpCodes.Ret);
        }

        private void DefineToEntity(TypeDefinition type, TypeReference entityType, VersionModel versionModel)
        {
            var method = type.DefineMethod(VersionEntityToEntityName, MethodAttributes.Public | MethodAttributes.Virtual, entityType);
            var il = method.Body.GetILProcessor();

            // var entity = new Entity();
            var entity = new VariableDefinition(entityType);
            method.Body.Variables.Add(entity);

            il.Emit(OpCodes.Newobj, _module.ImportReference(versionModel.EntityType.GetConstructor()));
            il.Emit(OpCodes.Stloc, entity);

            foreach (var fieldMapping in versionModel.EntityFieldMappings)
            {
                // entity.A = A;
                var entityField = fieldMapping.Key.PropertyInfo;
                var historyField = fieldMapping.Value;

                il.Emit(OpCodes.Ldloc, entity);
                il.Emit(OpCodes.Ldarg_0); // this
                il.Emit(OpCodes.Call, historyField.GetMethod);
                il.Emit(OpCodes.Callvirt, _module.ImportReference(entityField.SetMethod));
            }

            // return entity
            il.Emit(OpCodes.Ldloc, entity);
            il.Emit(OpCodes.Ret);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MethodAttributes = Mono.Cecil.MethodAttributes;
using TypeAttributes = Mono.Cecil.TypeAttributes;

namespace Orbital.Data.Versioning
{
    internal class VersionModelCustomizer : ModelCustomizer
    {
        public static readonly string ModelMappingAnnotation = "VersionModelMapping";

        public override void Customize(ModelBuilder modelBuilder, DbContext dbContext)
        {
            base.Customize(modelBuilder, dbContext);

            var assemblyBuilder = AssemblyDefinition.CreateAssembly(new AssemblyNameDefinition("Autogen.Versioning", new Version(1, 0)), "Autogen.Versioning", ModuleKind.Dll);
            var entityBuilderMappings = new List<(IEntityType, TypeDefinition, IReadOnlyDictionary<IProperty, PropertyDefinition>)>();

            var entityModels = modelBuilder.Model.GetEntityTypes().ToList();
            foreach (var entityModel in entityModels)
            {
                var (versionEntityType, fieldMappings) = CreateVersionEntity(entityModel, assemblyBuilder.MainModule);
                entityBuilderMappings.Add((entityModel, versionEntityType, fieldMappings));
            }

            Assembly asm;
            using (var memoryStream = new MemoryStream())
            {
                assemblyBuilder.Write(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                asm = AssemblyLoadContext.Default.LoadFromStream(memoryStream);
            }

            var modelMapping = new Dictionary<string, VersionEntityMapping>();

            foreach (var (entityModel, versionEntityBuilder, fieldBuilderMappings) in entityBuilderMappings)
            {
                var versionEntityType = asm.GetType(versionEntityBuilder.FullName);

                modelBuilder.Entity(versionEntityType, entityBuilder =>
                {
                    var originalTableName = entityModel.FindAnnotation("Relational:TableName")?.Value ?? entityModel.ClrType.Name;
                    entityBuilder.HasAnnotation("Relational:TableName", originalTableName + "_history");
                });

                modelMapping.Add(entityModel.ClrType.Name, new VersionEntityMapping(entityModel.ClrType, versionEntityType, fieldBuilderMappings));
            }

            modelBuilder.Model.AddAnnotation(ModelMappingAnnotation, modelMapping);
        }

        private static (TypeDefinition, IReadOnlyDictionary<IProperty, PropertyDefinition>) CreateVersionEntity(IMutableEntityType entityModel, ModuleDefinition moduleBuilder)
        {
            var entityType = moduleBuilder.ImportReference(entityModel.ClrType);

            var typeBuilder = new TypeDefinition("Autogen.Versioning", entityType.Name + "Version", TypeAttributes.Class | TypeAttributes.Public, moduleBuilder.TypeSystem.Object);
            moduleBuilder.Types.Add(typeBuilder);

            var versionEntityInterface = moduleBuilder.ImportReference(typeof(IVersionEntity<>));
            var versionEntity = new GenericInstanceType(versionEntityInterface);
            versionEntity.GenericArguments.Add(entityType);

            // Implement IVersionEntity<TEntity>
            typeBuilder.Interfaces.Add(new InterfaceImplementation(versionEntity));

            // Id & Date columns
            DefineColumnProperty(typeBuilder, nameof(IVersionEntity<object>.Id), moduleBuilder.TypeSystem.Int64);
            var dateColumn = DefineColumnProperty(typeBuilder, nameof(IVersionEntity<object>.Date), moduleBuilder.ImportReference(typeof(DateTime)));

            // Copy all the non-navigation columns from the entity, prefix the with Field_ to avoid collisions with version columns
            var fieldMappings = new Dictionary<IProperty, PropertyDefinition>();
            foreach (var prop in entityModel.GetProperties())
            {
                var property = DefineColumnProperty(typeBuilder, "Field_" + prop.Name, moduleBuilder.ImportReference(prop.ClrType));
                fieldMappings.Add(prop, property);
            }

            // Define the default & entity constructors
            DefineConstructors(typeBuilder, entityType, dateColumn, fieldMappings);
            // Finish implementing IVersionEntity<TEntity>
            DefineToEntity(typeBuilder, entityType, moduleBuilder.ImportReference(entityModel.ClrType.GetTypeInfo().GetConstructor(Type.EmptyTypes)), fieldMappings);

            return (typeBuilder, fieldMappings);
        }

        private static void DefineConstructors(TypeDefinition typeBuilder, TypeReference entityType, PropertyDefinition dateColumn, Dictionary<IProperty, PropertyDefinition> fieldMappings)
        {
            const MethodAttributes constructorAttributes = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;

            // default constructor
            var defaultContructor = new MethodDefinition(".ctor", constructorAttributes, typeBuilder.Module.TypeSystem.Void);
            typeBuilder.Methods.Add(defaultContructor);

            var defaultConstructorIL = defaultContructor.Body.GetILProcessor();

            defaultConstructorIL.Emit(OpCodes.Ldarg_0);
            defaultConstructorIL.Emit(OpCodes.Call, typeBuilder.Module.ImportReference(typeof(object).GetTypeInfo().GetConstructor(Type.EmptyTypes)));
            defaultConstructorIL.Emit(OpCodes.Ret);

            // from entity constructor
            var entityContructor = new MethodDefinition(".ctor", constructorAttributes, typeBuilder.Module.TypeSystem.Void);
            var entity = new ParameterDefinition(entityType);
            entityContructor.Parameters.Add(entity);
            typeBuilder.Methods.Add(entityContructor);

            var entityContructorIL = entityContructor.Body.GetILProcessor();

            // base()
            entityContructorIL.Emit(OpCodes.Ldarg_0); // this
            entityContructorIL.Emit(OpCodes.Call, typeBuilder.Module.ImportReference(typeof(object).GetTypeInfo().GetConstructor(Type.EmptyTypes)));

            // this.Date = DateTime.UtcNow;
            entityContructorIL.Emit(OpCodes.Ldarg_0); // this
            entityContructorIL.Emit(OpCodes.Call, typeBuilder.Module.ImportReference(typeof(DateTime).GetRuntimeProperty(nameof(DateTime.UtcNow)).GetMethod));
            entityContructorIL.Emit(OpCodes.Call, dateColumn.SetMethod);

            foreach (var fieldMapping in fieldMappings)
            {
                // this.A = entity.A;
                var entityField = fieldMapping.Key.PropertyInfo;
                var historyField = fieldMapping.Value;

                entityContructorIL.Emit(OpCodes.Ldarg_0); // this
                entityContructorIL.Emit(OpCodes.Ldarg, entity);
                entityContructorIL.Emit(OpCodes.Callvirt, typeBuilder.Module.ImportReference(entityField.GetMethod));
                entityContructorIL.Emit(OpCodes.Call, historyField.SetMethod);
            }

            // return
            entityContructorIL.Emit(OpCodes.Ret);
        }

        private static void DefineToEntity(TypeDefinition typeBuilder, TypeReference entityType, MethodReference entityConstructor, Dictionary<IProperty, PropertyDefinition> fieldMappings)
        {
            var method = new MethodDefinition(nameof(IVersionEntity<object>.ToEntity), MethodAttributes.Public | MethodAttributes.Virtual, entityType);
            typeBuilder.Methods.Add(method);
            var il = method.Body.GetILProcessor();

            // var entity = new Entity();
            var entity = new VariableDefinition(entityType);
            method.Body.Variables.Add(entity);

            il.Emit(OpCodes.Newobj, entityConstructor);
            il.Emit(OpCodes.Stloc, entity);

            foreach (var fieldMapping in fieldMappings)
            {
                // entity.A = A;
                var entityField = fieldMapping.Key.PropertyInfo;
                var historyField = fieldMapping.Value;

                il.Emit(OpCodes.Ldloc, entity);
                il.Emit(OpCodes.Ldarg_0); // this
                il.Emit(OpCodes.Call, historyField.GetMethod);
                il.Emit(OpCodes.Callvirt, typeBuilder.Module.ImportReference(entityField.SetMethod));
            }

            // return entity
            il.Emit(OpCodes.Ldloc, entity);
            il.Emit(OpCodes.Ret);

            //TODO is this needed? 
            // typeBuilder.DefineMethodOverride(
            //    methodBuilder,
            //    typeof(IVersionEntity<>).MakeGenericType(entityType).GetRuntimeMethod(nameof(IVersionEntity<object>.ToEntity), Type.EmptyTypes));
        }

        private static PropertyDefinition DefineColumnProperty(TypeDefinition typeBuilder, string id, TypeReference type)
        {
            var columnAttributeConstructor = type.Module.ImportReference(typeof(ColumnAttribute).GetTypeInfo().GetConstructor(new[] { typeof(string) }));
            var columnAttribute = new CustomAttribute(columnAttributeConstructor);
            columnAttribute.ConstructorArguments.Add(new CustomAttributeArgument(typeBuilder.Module.TypeSystem.String, id));

            var property = typeBuilder.DefineAutoProperty(id, type);
            property.CustomAttributes.Add(columnAttribute);

            return property;
        }
    }
}
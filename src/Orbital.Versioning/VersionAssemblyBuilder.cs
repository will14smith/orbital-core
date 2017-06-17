using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MethodAttributes = Mono.Cecil.MethodAttributes;
using TypeAttributes = Mono.Cecil.TypeAttributes;

namespace Orbital.Versioning
{
    internal class VersionAssemblyBuilder
    {
        private readonly AssemblyDefinition _assembly;
        private readonly ModuleDefinition _module;

        private readonly TypeReference _versionEntityGenericDefinition;
        private readonly TypeReference _versionMetadataGenericDefinition;

        private const string VersionEntityIdName = nameof(IVersionEntity<object>.Id);
        private const string VersionEntityDateName = nameof(IVersionEntity<object>.Date);
        private const string VersionEntityToEntityName = nameof(IVersionEntity<object>.ToEntity);

        public VersionAssemblyBuilder(string assemblyName = "Autogen.Versioning")
        {
            _assembly = AssemblyDefinition.CreateAssembly(new AssemblyNameDefinition(assemblyName, new Version(1, 0)), assemblyName, ModuleKind.Dll);
            _module = _assembly.MainModule;

            _versionEntityGenericDefinition = _module.ImportReference(typeof(IVersionEntity<>));
            _versionMetadataGenericDefinition = _module.ImportReference(typeof(IVersionEntityWithMetadata<>));
            _versionMetadataGenericDefinition.GenericParameters.Add(new GenericParameter("TMetadata", _versionMetadataGenericDefinition));
        }

        public Assembly Build()
        {
            return _assembly.Build();
        }

        public VersionModel Add(EntityModel entityModel, IReadOnlyCollection<IVersionMetadataProvider> metadata)
        {
            var entityType = _module.ImportReference(entityModel.EntityType);
            var versionType = _module.DefineType(entityType.Name + "Version", TypeAttributes.Class | TypeAttributes.Public, _module.TypeSystem.Object);

            // Implement IVersionEntity<TEntity>
            var versionEntity = new GenericInstanceType(_versionEntityGenericDefinition);
            versionEntity.GenericArguments.Add(entityType);
            versionType.Interfaces.Add(new InterfaceImplementation(versionEntity));

            // Id & Date columns
            var idColumn = DefineColumnProperty(versionType, VersionEntityIdName, _module.TypeSystem.Int64);
            var dateColumn = DefineColumnProperty(versionType, VersionEntityDateName, _module.ImportReference(typeof(DateTime)));

            // Copy all the non-navigation columns from the entity, prefix the with Field_ to avoid collisions with version columns
            var fieldMappings = new Dictionary<PropertyInfo, PropertyDefinition>();
            foreach (var prop in entityModel.Properties)
            {
                var property = DefineColumnProperty(versionType, "Field_" + prop.Name, _module.ImportReference(prop.PropertyType));
                fieldMappings.Add(prop, property);
            }

            var versionModel = new VersionModel(
                entityType, versionType,
                idColumn, dateColumn,
                entityModel, fieldMappings);

            // Implement all the metadata
            DefineMetadata(metadata, versionType, versionModel);

            // Define the default & entity constructors
            DefineConstructors(versionType, entityType, versionModel);
            // Finish implementing IVersionEntity<TEntity>
            DefineToEntity(versionType, entityType, versionModel);


            return versionModel;
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

        public class X : IVersionEntityWithMetadata<Entity>
        {
            Entity IVersionEntityWithMetadata<Entity>.ToMetadata()
            {
                return null;
            }
        }

        public class Entity
        {
            public int A { get; set; }
        }

        private void DefineMetadata(IEnumerable<IVersionMetadataProvider> metadataProviders, TypeDefinition type, VersionModel versionModel)
        {
            foreach (var metadataProvider in metadataProviders)
            {
                // Does this metadataProvider apply to this entity?
                if (!metadataProvider.AppliesTo(versionModel.EntityModel))
                {
                    continue;
                }

                var metadataType = _module.ImportReference(metadataProvider.MetadataType);

                // map the metadata fields to the version entity
                var mapping = new Dictionary<PropertyInfo, PropertyDefinition>();
                foreach (var property in metadataProvider.Properties)
                {
                    var column = DefineColumnProperty(type, $"Metadata_{metadataProvider.Name}_{property.Name}", _module.ImportReference(property.PropertyType));

                    mapping.Add(property, column);
                }

                // register in the model
                versionModel.AddMetadataProvider(metadataProvider, mapping);

                // implement IVersionEntityWithMetadata<TMetadata>
                var versionEntity = new GenericInstanceType(_versionMetadataGenericDefinition);
                versionEntity.GenericArguments.Add(metadataType);
                type.Interfaces.Add(new InterfaceImplementation(versionEntity));

                // HACK: get around the fact that Mono.Cecil doesn't support importing method from generic types yet
                var toMetadataName = nameof(IVersionEntityWithMetadata<int>.ToMetadata);
                var declaringType = _module.ImportReference(typeof(IVersionEntityWithMetadata<>).MakeGenericType(metadataProvider.MetadataType));
                var toMetadata = type.DefineMethod(declaringType.Name + "." + toMetadataName, MethodAttributes.Private | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual, metadataType);
                toMetadata.Overrides.Add(new MethodReference(toMetadataName, _module.ImportReference(typeof(IVersionEntityWithMetadata<>).GetTypeInfo().GenericTypeParameters[0], _versionMetadataGenericDefinition), declaringType) { HasThis = true });

                var il = toMetadata.Body.GetILProcessor();

                var metadata = new VariableDefinition(metadataType);
                toMetadata.Body.Variables.Add(metadata);

                il.Emit(OpCodes.Newobj, _module.ImportReference(versionModel.EntityType.GetConstructor()));
                il.Emit(OpCodes.Stloc, metadata);

                foreach (var fieldMapping in mapping)
                {
                    // metadata.A = A;
                    var metadataField = fieldMapping.Key;
                    var versionField = fieldMapping.Value;

                    il.Emit(OpCodes.Ldloc, metadata);
                    il.Emit(OpCodes.Ldarg_0); // this
                    il.Emit(OpCodes.Call, versionField.GetMethod);
                    il.Emit(OpCodes.Callvirt, _module.ImportReference(metadataField.SetMethod));
                }

                // return entity
                il.Emit(OpCodes.Ldloc, metadata);
                il.Emit(OpCodes.Ret);
            }
        }

        private void DefineConstructors(TypeDefinition type, TypeReference entityType, VersionModel versionModel)
        {
            var defaultConstructor = type.DefineDefaultConstructor();

            // entity+metadata constructor
            var entity = new ParameterDefinition(entityType);
            var metadataParameters = versionModel.MetadataProviders.Select(x => new ParameterDefinition(_module.ImportReference(x.MetadataProvider.MetadataType))).ToArray();
            var entityContructor = type.DefineConstructor(new[] { entity }.Concat(metadataParameters).ToArray());

            var entityContructorIL = entityContructor.Body.GetILProcessor();

            // base()
            entityContructorIL.Emit(OpCodes.Ldarg_0); // this
            entityContructorIL.Emit(OpCodes.Call, defaultConstructor);

            // this.Date = DateTime.UtcNow;
            entityContructorIL.Emit(OpCodes.Ldarg_0); // this
            entityContructorIL.Emit(OpCodes.Call, _module.ImportReference(typeof(DateTime).GetRuntimeProperty(nameof(DateTime.UtcNow)).GetMethod));
            entityContructorIL.Emit(OpCodes.Call, versionModel.DateColumn.SetMethod);

            // entity
            foreach (var fieldMapping in versionModel.EntityFieldMappings)
            {
                // this.A = entity.A;
                var entityProperty = fieldMapping.Key;
                var versionProperty = fieldMapping.Value;

                entityContructorIL.Emit(OpCodes.Ldarg_0); // this
                entityContructorIL.Emit(OpCodes.Ldarg, entity);
                entityContructorIL.Emit(OpCodes.Callvirt, _module.ImportReference(entityProperty.GetMethod));
                entityContructorIL.Emit(OpCodes.Call, versionProperty.SetMethod);
            }

            // metadata
            foreach (var (metadataModel, parameter) in versionModel.MetadataProviders.Zip(metadataParameters, (a, b) => (a, b)))
            {
                foreach (var fieldMapping in metadataModel.FieldMappings)
                {
                    // this.A = metadata.A;
                    var metadataProperty = fieldMapping.Key;
                    var versionProperty = fieldMapping.Value;

                    entityContructorIL.Emit(OpCodes.Ldarg_0); // this
                    entityContructorIL.Emit(OpCodes.Ldarg, parameter);
                    entityContructorIL.Emit(OpCodes.Callvirt, _module.ImportReference(metadataProperty.GetMethod));
                    entityContructorIL.Emit(OpCodes.Call, versionProperty.SetMethod);
                }
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
                var entityField = fieldMapping.Key;
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

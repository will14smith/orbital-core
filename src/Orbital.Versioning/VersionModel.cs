using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;

namespace Orbital.Versioning
{
    internal class VersionModel
    {
        public TypeReference EntityReference { get; }
        public TypeReference VersionReference { get; }

        public PropertyDefinition IdColumn { get; }
        public PropertyDefinition DateColumn { get; }

        public EntityModel EntityModel { get; }
        public IReadOnlyDictionary<PropertyInfo, PropertyDefinition> EntityFieldMappings { get; }

        public IReadOnlyCollection<MetadataModel> MetadataModels => _metadataModels;

        public Type EntityType => EntityModel.EntityType;
        public Type VersionType => Assembly == null
            ? throw new InvalidOperationException("Internal: Assembly is not set yet.")
            : Assembly.GetType(VersionReference.FullName) ?? throw new Exception($"Couldn't find version type ({VersionReference.FullName}) in assembly.");

        internal Assembly Assembly;
        private readonly List<MetadataModel> _metadataModels = new List<MetadataModel>();

        public VersionModel(
            TypeReference entityReference, TypeReference versionReference,
            PropertyDefinition idColumn, PropertyDefinition dateColumn,
            EntityModel entityModel, IReadOnlyDictionary<PropertyInfo, PropertyDefinition> entityFieldMappings)
        {
            EntityReference = entityReference;
            VersionReference = versionReference;

            IdColumn = idColumn;
            DateColumn = dateColumn;

            EntityFieldMappings = entityFieldMappings.ToDictionary(x => x.Key, x => x.Value, new WeakPropertyInfoEqualityComparer());
            EntityModel = entityModel;
        }

        public bool TryFindMapping(MemberInfo entityMember, out PropertyInfo versionMember)
        {
            if (!(entityMember is PropertyInfo entityProperty))
            {
                versionMember = null;
                return false;
            }

            if (!EntityFieldMappings.TryGetValue(entityProperty, out var versionMemberReference))
            {
                versionMember = null;
                return false;
            }

            versionMember = VersionType.GetRuntimeProperty(versionMemberReference.Name);
            return versionMember != null;
        }

        internal void AddMetadataExtension(IVersionMetadataExtension metadataExtension, IReadOnlyDictionary<PropertyInfo, PropertyDefinition> fieldMappings)
        {
            if (_metadataModels.Any(x => x.MetadataExtension.Name == metadataExtension.Name))
            {
                throw new InvalidOperationException($"Duplicate metadata extension with name {metadataExtension.Name}");
            }

            _metadataModels.Add(new MetadataModel(metadataExtension, fieldMappings));
        }
    }
}
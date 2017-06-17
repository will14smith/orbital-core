using System;
using System.Collections.Generic;
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

        public Type EntityType => EntityModel.EntityType;
        public Type VersionType => Assembly == null
            ? throw new InvalidOperationException("Internal: Assembly is not set yet.")
            : Assembly.GetType(VersionReference.FullName) ?? throw new Exception($"Couldn't find version type ({VersionReference.FullName}) in assembly.");

        internal Assembly Assembly;

        public VersionModel(
            TypeReference entityReference, TypeReference versionReference,
            PropertyDefinition idColumn, PropertyDefinition dateColumn,
            EntityModel entityModel, IReadOnlyDictionary<PropertyInfo, PropertyDefinition> entityFieldMappings)
        {
            EntityReference = entityReference;
            VersionReference = versionReference;

            IdColumn = idColumn;
            DateColumn = dateColumn;

            EntityFieldMappings = entityFieldMappings;
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
    }
}
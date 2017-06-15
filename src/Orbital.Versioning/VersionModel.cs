using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;
using Mono.Cecil;

namespace Orbital.Versioning
{
    internal class VersionModel
    {
        public TypeReference EntityReference { get; }
        public TypeReference VersionReference { get; }

        public PropertyDefinition IdColumn { get; }
        public PropertyDefinition DateColumn { get; }

        public IEntityType EntityModel { get; }
        public IReadOnlyDictionary<IProperty, PropertyDefinition> EntityFieldMappings { get; }

        public Type EntityType => EntityModel.ClrType;
        public Type VersionType => Assembly == null
            ? throw new InvalidOperationException("Internal: Assembly is not set yet.")
            : Assembly.GetType(VersionReference.FullName) ?? throw new Exception($"Couldn't find version type ({VersionReference.FullName}) in assembly.");

        internal Assembly Assembly;

        public VersionModel(
            TypeReference entityReference, TypeReference versionReference,
            PropertyDefinition idColumn, PropertyDefinition dateColumn,
            IEntityType entityModel, IReadOnlyDictionary<IProperty, PropertyDefinition> entityFieldMappings)
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
            var versionMemberReference = EntityFieldMappings.Where(x => x.Key.PropertyInfo == entityMember).Select(x => x.Value).FirstOrDefault();
            if (versionMemberReference == null)
            {
                versionMember = null;
                return false;
            }

            versionMember = VersionType.GetRuntimeProperty(versionMemberReference.Name);
            return true;
        }
    }
}
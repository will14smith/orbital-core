using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;
using Mono.Cecil;

namespace Orbital.Data.Versioning
{
    public class VersionEntityMapping
    {
        public VersionEntityMapping(Type entityType, Type versionEntityType, IReadOnlyDictionary<IProperty, PropertyDefinition> fieldMappings)
        {
            EntityType = entityType;
            VersionEntityType = versionEntityType;
            // Convert to runtime property
            FieldMappings = fieldMappings.ToDictionary(x => x.Key, x => versionEntityType.GetRuntimeProperty(x.Value.Name));
        }

        public Type EntityType { get; }
        public Type VersionEntityType { get; }

        // From Entity to VersionEntity fields
        public IReadOnlyDictionary<IProperty, PropertyInfo> FieldMappings { get; }

        public PropertyInfo FindMapping(MemberInfo entityMember)
        {
            return FieldMappings.Where(x => x.Key.PropertyInfo == entityMember).Select(x => x.Value).FirstOrDefault();
        }
    }
}
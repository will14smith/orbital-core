using System;
using System.Collections.Generic;
using System.Reflection;

namespace Orbital.Versioning
{
    public class EntityModel
    {
        public Type EntityType { get; }
        public IReadOnlyCollection<PropertyInfo> Properties { get; }

        public EntityModel(Type entityType, IReadOnlyCollection<PropertyInfo> properties)
        {
            EntityType = entityType;
            Properties = properties;
        }
    }
}

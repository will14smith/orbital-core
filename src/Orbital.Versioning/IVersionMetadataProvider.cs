using System;
using System.Collections.Generic;
using System.Reflection;

namespace Orbital.Versioning
{
    public interface IVersionMetadataProvider
    {
        string Name { get; }
        Type MetadataType { get; }
        IReadOnlyCollection<PropertyInfo> Properties { get; }

        bool AppliesTo(EntityModel entity);

        object GetMetadata();
    }
    public interface IVersionMetadataProvider<out TMetadata> : IVersionMetadataProvider
    {
        new TMetadata GetMetadata();
    }
}

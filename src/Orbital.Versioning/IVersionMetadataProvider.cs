using System;
using System.Collections.Generic;
using System.Reflection;

namespace Orbital.Versioning
{
    public interface IVersionMetadataExtension
    {
        string Name { get; }
        Type MetadataType { get; }
        IReadOnlyCollection<PropertyInfo> Properties { get; }

        bool AppliesTo(EntityModel entity);

        IVersionMetadataProvider GetProvider(IServiceProvider services);
    }
    public interface IVersionMetadataExtension<out TMetadata> : IVersionMetadataExtension
    {
        new IVersionMetadataProvider<TMetadata> GetProvider(IServiceProvider services);
    }
    
    public interface IVersionMetadataProvider
    {
        object GetMetadata();
    }
    public interface IVersionMetadataProvider<out TMetadata> : IVersionMetadataProvider
    {
        new TMetadata GetMetadata();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Orbital.Versioning
{
    public class VersionMetadataExtension<TMetadata> : ReflectionVersionMetadataExtension<TMetadata>
    {
        private readonly Func<IServiceProvider, TMetadata> _valueProvider;

        public VersionMetadataExtension(Func<IServiceProvider, TMetadata> valueProvider)
        {
            _valueProvider = valueProvider;
        }

        public override IVersionMetadataProvider<TMetadata> GetProvider(IServiceProvider services)
        {
            return new VersionMetadataStaticProvider<TMetadata>(_valueProvider(services));
        }
    }

    public class VersionMetadataStaticProvider<TMetadata> : IVersionMetadataProvider<TMetadata>
    {
        private readonly TMetadata _value;

        public VersionMetadataStaticProvider(TMetadata value)
        {
            _value = value;
        }

        public TMetadata GetMetadata()
        {
            return _value;
        }

        object IVersionMetadataProvider.GetMetadata()
        {
            return GetMetadata();
        }
    }

    public abstract class ReflectionVersionMetadataExtension<TMetadata> : IVersionMetadataExtension<TMetadata>
    {
        public virtual string Name => MetadataType.Name;
        public virtual Type MetadataType => typeof(TMetadata);
        public virtual IReadOnlyCollection<PropertyInfo> Properties => MetadataType.GetRuntimeProperties().ToList();


        public virtual bool AppliesTo(EntityModel entity)
        {
            return true;
        }

        public abstract IVersionMetadataProvider<TMetadata> GetProvider(IServiceProvider services);

        IVersionMetadataProvider IVersionMetadataExtension.GetProvider(IServiceProvider services)
        {
            return GetProvider(services);
        }
    }
}

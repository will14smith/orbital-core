using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Orbital.Versioning
{
    public class VersionMetadataProvider<TMetadata> : ReflectionVersionMetadataProvider<TMetadata>
    {
        private readonly Func<TMetadata> _valueProvider;

        public VersionMetadataProvider(Func<TMetadata> valueProvider)
        {
            _valueProvider = valueProvider;
        }

        public override TMetadata GetMetadata()
        {
            return _valueProvider();
        }
    }
    public abstract class ReflectionVersionMetadataProvider<TMetadata> : IVersionMetadataProvider<TMetadata>
    {
        public virtual string Name => MetadataType.Name;
        public virtual Type MetadataType => typeof(TMetadata);
        public virtual IReadOnlyCollection<PropertyInfo> Properties => MetadataType.GetRuntimeProperties().ToList();


        public virtual bool AppliesTo(EntityModel entity)
        {
            return true;
        }

        public abstract TMetadata GetMetadata();

        object IVersionMetadataProvider.GetMetadata()
        {
            return GetMetadata();
        }
    }
}

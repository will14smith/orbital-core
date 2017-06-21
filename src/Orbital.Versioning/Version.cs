using System;
using System.Collections.Generic;

namespace Orbital.Versioning
{
    public class Version<T>
    {
        private readonly IReadOnlyDictionary<string, object> _metadata;

        public Version(IVersionEntity<T> versionEntity)
        {
            VersionId = versionEntity.Id;
            Date = versionEntity.Date;
            Entity = versionEntity.ToEntity();

            _metadata = versionEntity.ToMetadata();
        }

        public long VersionId { get; }
        public DateTime Date { get; }

        public T Entity { get; }

        public TMetadata GetMetadata<TMetadata>(string name)
        {
            if (!_metadata.TryGetValue(name, out var value))
            {
                throw new InvalidOperationException($"Metadata with a name of '{name}' was not found.");
            }

            if (!(value is TMetadata metadata))
            {
                throw new InvalidOperationException($"Metadata was null or not type '{typeof(TMetadata).Name}'");
            }

            return metadata;
        }
    }
}
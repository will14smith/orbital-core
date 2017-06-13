using System;

namespace Orbital.Data.Versioning
{
    public class Version<T>
    {
        public Version(long versionId, DateTime date, T entity)
        {
            VersionId = versionId;
            Date = date;
            Entity = entity;
        }
        public Version(IVersionEntity<T> versionEntity)
            : this(versionEntity.Id, versionEntity.Date, versionEntity.ToEntity())
        {
        }

        public long VersionId { get; }
        public DateTime Date { get; }

        public T Entity { get; }
    }
}
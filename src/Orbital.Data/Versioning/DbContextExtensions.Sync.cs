using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Orbital.Data.Versioning
{
    public static partial class DbContextExtensions
    {
        public static void SyncVersion(this DbContext context)
        {
            var changeEntries = context.ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted)
                .ToList();

            var versionEntityMappings = context.GetVersionEntityMappings();

            foreach (var changeEntry in changeEntries)
            {
                var entity = changeEntry.CurrentValues.ToObject();
                if (!versionEntityMappings.TryGetValue(changeEntry.Metadata.ClrType.Name, out var versionEntityMapping))
                {
                    throw new InvalidOperationException($"Couldn't find entity mapping for {changeEntry.Metadata.ClrType.Name}");
                }

                context.Add(Activator.CreateInstance(versionEntityMapping.VersionEntityType, entity));
            }
        }
    }
}

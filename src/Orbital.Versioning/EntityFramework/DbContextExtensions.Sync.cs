using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Orbital.Versioning
{
    public static partial class DbContextExtensions
    {
        public static void SyncVersion(this DbContext context)
        {
            var changeEntries = context.ChangeTracker.Entries().Where(ShouldTrack);
            var versionEntityMappings = context.GetVersionModels();

            var versionEntitiesToAdd = new List<object>();

            foreach (var changeEntry in changeEntries)
            {
                var entity = changeEntry.CurrentValues.ToObject();
                var entityTypeName = changeEntry.Metadata.ClrType.FullName;

                if (!versionEntityMappings.TryGetValue(entityTypeName, out var versionEntityMapping))
                {
                    throw new InvalidOperationException($"Couldn't find entity mapping for {entityTypeName}");
                }

                versionEntitiesToAdd.Add(Activator.CreateInstance(versionEntityMapping.VersionType, entity));
            }

            context.AddRange(versionEntitiesToAdd);
        }

        private static bool ShouldTrack(EntityEntry entry)
        {
            var state = entry.State;

            return state == EntityState.Added
                || state == EntityState.Modified
                || state == EntityState.Deleted;
        }
    }
}

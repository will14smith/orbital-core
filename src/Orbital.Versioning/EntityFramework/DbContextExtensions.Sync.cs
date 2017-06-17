using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Orbital.Versioning
{
    public static partial class DbContextExtensions
    {
        public static void SyncVersioning(this DbContext context)
        {
            var changeEntries = context.ChangeTracker.Entries().Where(ShouldTrack);
            var versionModels = context.GetVersionModels();

            var versionEntitiesToAdd = new List<object>();

            foreach (var changeEntry in changeEntries)
            {
                var entity = changeEntry.CurrentValues.ToObject();
                var entityTypeName = changeEntry.Metadata.ClrType.FullName;

                if (!versionModels.TryGetValue(entityTypeName, out var versionModel))
                {
                    throw new InvalidOperationException($"Couldn't find version model for {entityTypeName}");
                }

                var constructorArguments = new List<object> { entity };
                foreach (var metadataProvider in versionModel.MetadataModels)
                {
                    var metadata = metadataProvider.MetadataProvider.GetMetadata();

                    constructorArguments.Add(metadata);
                }

                versionEntitiesToAdd.Add(Activator.CreateInstance(versionModel.VersionType, constructorArguments.ToArray()));
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

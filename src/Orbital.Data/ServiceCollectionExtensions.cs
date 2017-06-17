using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orbital.Versioning;

namespace Orbital.Data
{
    public static class ServiceCollectionExtensions
    {
        public static void AddOrbitalData(this IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddDbContext<OrbitalContext>(options =>
            {
                options
                    .UseVersioning(versionOptions =>
                    {
                        // TODO user tracking
                    })
                    .UseNpgsql(connectionString);
            });

            // TODO register services
        }

        public static void MigrateOrbitalData(this IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<OrbitalContext>();

            context.Database.Migrate();
        }
    }
}

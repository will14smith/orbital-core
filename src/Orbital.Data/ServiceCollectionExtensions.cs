using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace Orbital.Data
{
    public static class ServiceCollectionExtensions
    {
        public static void AddOrbitalData(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> dbOptionsAction)
        {
            serviceCollection.AddDbContext<OrbitalContext>(dbOptionsAction);
        }

        public static void MigrateOrbitalData(this IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<OrbitalContext>();

            context.Database.Migrate();
        }
    }
}

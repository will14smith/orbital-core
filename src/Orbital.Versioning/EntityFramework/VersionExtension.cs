using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Orbital.Versioning
{
    public class VersionExtension : IDbContextOptionsExtension
    {
        public void ApplyServices(IServiceCollection services)
        {
            services.AddTransient<IModelCustomizer, VersionModelCustomizer>();
        }
    }

    public static class VersionDbContextOptionsExtensions
    {
        public static DbContextOptionsBuilder<TContext> UseVersioning<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder)
            where TContext : DbContext
            => (DbContextOptionsBuilder<TContext>)UseVersioning((DbContextOptionsBuilder)optionsBuilder);

        public static DbContextOptionsBuilder UseVersioning(this DbContextOptionsBuilder optionsBuilder)
        {
            var existing = optionsBuilder.Options.FindExtension<VersionExtension>();
            if (existing != null)
            {
                throw new InvalidOperationException("Cannot configure VersionExtension twice");
            }

            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(new VersionExtension());

            return optionsBuilder;
        }
    }
}
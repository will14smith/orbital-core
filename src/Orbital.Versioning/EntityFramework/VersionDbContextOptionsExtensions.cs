using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Orbital.Versioning
{
    public static class VersionDbContextOptionsExtensions
    {
        public static DbContextOptionsBuilder<TContext> UseVersioning<TContext>(
            this DbContextOptionsBuilder<TContext> optionsBuilder,
            Action<VersionDbContextOptionsBuilder> builder = null)
                where TContext : DbContext
            => (DbContextOptionsBuilder<TContext>)UseVersioning((DbContextOptionsBuilder)optionsBuilder, builder);

        public static DbContextOptionsBuilder UseVersioning(this DbContextOptionsBuilder optionsBuilder, Action<VersionDbContextOptionsBuilder> builder = null)
        {
            var extension = optionsBuilder.Options.FindExtension<VersionExtension>() ?? new VersionExtension();

            builder?.Invoke(new VersionDbContextOptionsBuilder(optionsBuilder, extension));

            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

            return optionsBuilder;
        }
    }
}
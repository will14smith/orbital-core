using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Orbital.Versioning
{
    public class VersionExtension : IDbContextOptionsExtension
    {
        internal readonly List<IVersionMetadataProvider> Metadata = new List<IVersionMetadataProvider>();

        public void ApplyServices(IServiceCollection services)
        {
            services.AddTransient<IModelCustomizer, VersionModelCustomizer>(x => new VersionModelCustomizer(Metadata));
        }
    }
}
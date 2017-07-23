using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Orbital.Versioning
{
    public class VersionExtension : IDbContextOptionsExtension
    {
        internal readonly List<IVersionMetadataExtension> Metadata = new List<IVersionMetadataExtension>();

        public bool ApplyServices(IServiceCollection services)
        {
            services.AddSingleton<VersionModelStore>();
            services.AddTransient<IModelCustomizer, VersionModelCustomizer>(x => 
                new VersionModelCustomizer(
                    Metadata,  
                    x.GetRequiredService<VersionModelStore>(), 
                    x.GetRequiredService<ModelCustomizerDependencies>()));

            return false;
        }

        public long GetServiceProviderHashCode()
        {
            return 0;
        }

        public void Validate(IDbContextOptions options)
        {
        }
    }
}
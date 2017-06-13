using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Orbital.Data.Versioning
{
    internal class VersionExtension : IDbContextOptionsExtension
    {
        public void ApplyServices(IServiceCollection services)
        {
            services.AddTransient<IModelCustomizer, VersionModelCustomizer>();
        }
    }
}
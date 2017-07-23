using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Orbital.Data.Entities;
using Orbital.Versioning;

namespace Orbital.Web.Helpers
{
    public class UserMetadataExtension : ReflectionVersionMetadataExtension<UserMetadata>
    {
        public override IVersionMetadataProvider<UserMetadata> GetProvider(IServiceProvider services)
        {
            return new UserMetadataProvider(services);
        }
    }

    public class UserMetadataProvider : IVersionMetadataProvider<UserMetadata>
    {
        private readonly ICurrentDbContext _currentContext;

        public UserMetadataProvider(IServiceProvider services)
        {
            _currentContext = services.GetRequiredService<IDbContextServices>().CurrentContext;
        }
        
        public UserMetadata GetMetadata()
        {
            return new UserMetadata
            {
                // TODO
                UserId = _currentContext.Context.Set<PersonEntity>().First().Id,
            };
        }

        object IVersionMetadataProvider.GetMetadata()
        {
            return GetMetadata();
        }
    }

    public static class UserMetadataProviderExtensions
    {
        public static UserMetadata GetUserMetadata<TEntity>(this Version<TEntity> version)
        {
            return version.GetMetadata<UserMetadata>(nameof(UserMetadata));
        }
    }

    public class UserMetadata
    {
        public Guid UserId { get; set; }
    }
}

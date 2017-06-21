using System;
using Orbital.Versioning;

namespace Orbital.Web.Helpers
{
    public class UserMetadataProvider : ReflectionVersionMetadataProvider<UserMetadata>
    {
        public override UserMetadata GetMetadata()
        {
            return new UserMetadata
            {
                // TODO
                UserId = Guid.NewGuid()
            };
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

using System;

namespace Orbital.Versioning.Tests
{
    public class UserMetadataProvider : ReflectionVersionMetadataProvider<UserMetadataProvider.UserMetadata>
    {
        public override UserMetadata GetMetadata()
        {
            return new UserMetadata { UserId = UserId };
        }

        public class UserMetadata
        {
            public Guid UserId { get; set; }
        }

        public static Guid UserId { get; set; }
    }

    public static class UserMetadataProviderExtensions
    {
        public static UserMetadataProvider.UserMetadata GetUserMetadata<TEntity>(this Version<TEntity> version)
        {
            return version.GetMetadata<UserMetadataProvider.UserMetadata>(nameof(UserMetadataProvider.UserMetadata));
        }
    }

}
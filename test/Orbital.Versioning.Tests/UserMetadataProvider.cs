using System;

namespace Orbital.Versioning.Tests
{
    public class UserMetadataExtension : ReflectionVersionMetadataExtension<UserMetadataProvider.UserMetadata>
    {
        public override IVersionMetadataProvider<UserMetadataProvider.UserMetadata> GetProvider(IServiceProvider services)
        {
            return new UserMetadataProvider();
        }
    }

    public class UserMetadataProvider : IVersionMetadataProvider<UserMetadataProvider.UserMetadata>
    {
        public UserMetadata GetMetadata()
        {
            return new UserMetadata { UserId = UserId };
        }
        object IVersionMetadataProvider.GetMetadata()
        {
            return GetMetadata();
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
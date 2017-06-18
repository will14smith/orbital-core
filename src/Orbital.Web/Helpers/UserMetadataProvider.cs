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

    public class UserMetadata
    {
        public Guid UserId { get; set; }
    }
}

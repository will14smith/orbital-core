using System;

namespace Orbital.Versioning.Tests
{
    public class IpAddressMetadataExtension : ReflectionVersionMetadataExtension<IpAddressMetadataProvider.IpAddressMetadata>
    {
        public override IVersionMetadataProvider<IpAddressMetadataProvider.IpAddressMetadata> GetProvider(IServiceProvider services)
        {
            return new IpAddressMetadataProvider();
        }
    }


    public class IpAddressMetadataProvider : IVersionMetadataProvider<IpAddressMetadataProvider.IpAddressMetadata>
    {
        public IpAddressMetadata GetMetadata()
        {
            return new IpAddressMetadata { IpAddress = IpAddress };
        }
        object IVersionMetadataProvider.GetMetadata()
        {
            return GetMetadata();
        }

        public class IpAddressMetadata
        {
            public string IpAddress { get; set; }
        }

        public static string IpAddress { get; set; }
    }
}
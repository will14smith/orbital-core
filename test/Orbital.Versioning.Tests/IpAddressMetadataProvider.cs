namespace Orbital.Versioning.Tests
{
    public class IpAddressMetadataProvider : ReflectionVersionMetadataProvider<IpAddressMetadataProvider.IpAddressMetadata>
    {
        public override IpAddressMetadata GetMetadata()
        {
            return new IpAddressMetadata { IpAddress = IpAddress };
        }


        public class IpAddressMetadata
        {
            public string IpAddress { get; set; }
        }

        public static string IpAddress { get; set; }
    }
}
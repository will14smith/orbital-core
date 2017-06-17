using System;
using System.Reflection;
using Xunit;

namespace Orbital.Versioning.Tests
{
    public partial class VersionAssemblyBuilderTests
    {
        [Fact]
        public void WithMetadata_DefaultConstructor_CanActivate()
        {
            var versionModel = _versionAssemblyBuilder.Add(_entityModel, new IVersionMetadataProvider[] { new UserMetadataProvider(), new IpAddressMetadataProvider() });
            var assembly = _versionAssemblyBuilder.Build();
            versionModel.Assembly = assembly;

            var instance = Activator.CreateInstance(versionModel.VersionType);

            Assert.NotNull(instance);
        }

        [Fact]
        public void WithMetadata_CopyConstructor_SetsMetadataFields()
        {
            var versionModel = _versionAssemblyBuilder.Add(_entityModel, new IVersionMetadataProvider[] { new UserMetadataProvider(), new IpAddressMetadataProvider() });
            var assembly = _versionAssemblyBuilder.Build();
            versionModel.Assembly = assembly;

            var userId = Guid.NewGuid();
            var ipAddress = "blah";

            var instance = Activator.CreateInstance(versionModel.VersionType, new Entity(), new UserMetadataProvider.UserMetadata { UserId = userId }, new IpAddressMetadataProvider.IpAddressMetadata { IpAddress = ipAddress });
            Assert.NotNull(instance);

            var versionType = versionModel.VersionType;

            var userIdProperty = versionType.GetRuntimeProperty($"Metadata_{nameof(UserMetadataProvider.UserMetadata)}_{nameof(UserMetadataProvider.UserMetadata.UserId)}");
            Assert.NotNull(userIdProperty);
            Assert.Equal(userId, userIdProperty.GetValue(instance));

            var ipAddressProperty = versionType.GetRuntimeProperty($"Metadata_{nameof(IpAddressMetadataProvider.IpAddressMetadata)}_{nameof(IpAddressMetadataProvider.IpAddressMetadata.IpAddress)}");
            Assert.NotNull(ipAddressProperty);
            Assert.Equal(ipAddress, ipAddressProperty.GetValue(instance));
        }

        [Fact]
        public void MetadataInterface_Implemented()
        {
            var versionModel = _versionAssemblyBuilder.Add(_entityModel, new IVersionMetadataProvider[] { new UserMetadataProvider(), new IpAddressMetadataProvider() });
            var assembly = _versionAssemblyBuilder.Build();
            versionModel.Assembly = assembly;

            var userId = Guid.NewGuid();
            var ipAddress = "blah";

            var instance = Activator.CreateInstance(versionModel.VersionType, new Entity(), new UserMetadataProvider.UserMetadata { UserId = userId }, new IpAddressMetadataProvider.IpAddressMetadata { IpAddress = ipAddress });

            var userIdVersionMetadata = Assert.IsAssignableFrom<IVersionEntityWithMetadata<UserMetadataProvider.UserMetadata>>(instance);
            Assert.Equal(userId, userIdVersionMetadata.ToMetadata().UserId);

            var ipAddressVersionMetadata = Assert.IsAssignableFrom<IVersionEntityWithMetadata<IpAddressMetadataProvider.IpAddressMetadata>>(instance);
            Assert.Equal(ipAddress, ipAddressVersionMetadata.ToMetadata().IpAddress);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Orbital.Versioning.Tests
{
    public partial class VersionAssemblyBuilderTests
    {
        private static readonly IVersionMetadataExtension[] Extensions = { new UserMetadataExtension(), new IpAddressMetadataExtension() };

        [Fact]
        public void WithMetadata_DefaultConstructor_CanActivate()
        {
            var versionModel = _versionAssemblyBuilder.Add(_entityModel, Extensions);
            var assembly = _versionAssemblyBuilder.Build();
            versionModel.Assembly = assembly;

            var instance = Activator.CreateInstance(versionModel.VersionType);

            Assert.NotNull(instance);
        }

        [Fact]
        public void WithMetadata_CopyConstructor_SetsMetadataFields()
        {
            var versionModel = _versionAssemblyBuilder.Add(_entityModel, Extensions);
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
            var versionModel = _versionAssemblyBuilder.Add(_entityModel, Extensions);
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

        [Fact]
        public void WithMetadata_VersionEntityInterface_ToMetadata_Works()
        {
            var versionModel = _versionAssemblyBuilder.Add(_entityModel, Extensions);
            var assembly = _versionAssemblyBuilder.Build();
            versionModel.Assembly = assembly;

            var userId = Guid.NewGuid();
            var ipAddress = "blah";

            var instance = Activator.CreateInstance(versionModel.VersionType, new Entity(), new UserMetadataProvider.UserMetadata { UserId = userId }, new IpAddressMetadataProvider.IpAddressMetadata { IpAddress = ipAddress });

            var toMetadata = typeof(IVersionEntity<Entity>).GetRuntimeMethod(nameof(IVersionEntity<Entity>.ToMetadata), Type.EmptyTypes);
            Assert.NotNull(toMetadata);

            var result = toMetadata.Invoke(instance, new object[0]);
            var metadataResult = Assert.IsAssignableFrom<IReadOnlyDictionary<string, object>>(result);

            var userMetadata = Assert.IsType<UserMetadataProvider.UserMetadata>(metadataResult[nameof(UserMetadataProvider.UserMetadata)]);
            Assert.Equal(userId, userMetadata.UserId);

            var ipAddressMetadata = Assert.IsType<IpAddressMetadataProvider.IpAddressMetadata>(metadataResult[nameof(IpAddressMetadataProvider.IpAddressMetadata)]);
            Assert.Equal(ipAddress, ipAddressMetadata.IpAddress);
        }
    }
}
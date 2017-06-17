using System;
using System.Reflection;
using Mono.Cecil;
using Xunit;

namespace Orbital.Versioning.Tests
{
    public class VersionModelTests
    {
        private static readonly Type T = typeof(VersionModelTests);
        private static readonly Assembly A = T.GetTypeInfo().Assembly;

        // EntityType
        [Fact]
        public void EntityType_ReturnsClrType()
        {
            var entityModel = new EntityModel(T, new PropertyInfo[0]);

            var model = new VersionModel(null, null, null, null, entityModel, null);

            Assert.Equal(T, model.EntityType);
        }

        // VersionType: no assembly, missing type, all happy
        [Fact]
        public void VersionType_AssemblyNotSet_ThrowsException()
        {
            var model = new VersionModel(null, null, null, null, null, null);

            Assert.Throws<InvalidOperationException>(() => model.VersionType);
        }
        [Fact]
        public void VersionType_TypeNotFound_ThrowsException()
        {
            var versionReference = new TypeReference("", "NotFound", null, null);

            var model = new VersionModel(null, versionReference, null, null, null, null)
            {
                Assembly = A
            };

            Assert.Throws<Exception>(() => model.VersionType);
        }
        [Fact]
        public void VersionType_ReturnsType()
        {
            var versionReference = new TypeReference(T.Namespace, T.Name, null, null);

            var model = new VersionModel(null, versionReference, null, null, null, null)
            {
                Assembly = A
            };

            Assert.Equal(T, model.VersionType);
        }

        // TryFindMapping: entityMember not found, version property not found, all happy
    }
}

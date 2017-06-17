using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Xunit;

namespace Orbital.Versioning.Tests
{
    public partial class VersionAssemblyBuilderTests
    {
        public class X : IVersionEntityWithMetadata<Entity>
        {
            Entity IVersionEntityWithMetadata<Entity>.ToMetadata()
            {
                return null;
            }
        }

        public class Entity
        {
            public int A { get; set; }
        }

        private readonly EntityModel _entityModel;
        private readonly VersionAssemblyBuilder _versionAssemblyBuilder;

        public VersionAssemblyBuilderTests()
        {
            var entityType = typeof(Entity);

            _entityModel = new EntityModel(
                entityType,
                new[]
                {
                    entityType.GetRuntimeProperty(nameof(Entity.A))
                });

            _versionAssemblyBuilder = new VersionAssemblyBuilder("Test_" + Guid.NewGuid().ToString("N"));
        }

        [Fact]
        public void DefaultConstructor_CanActivate()
        {
            var versionModel = _versionAssemblyBuilder.Add(_entityModel, new IVersionMetadataProvider[0]);
            var assembly = _versionAssemblyBuilder.Build();
            versionModel.Assembly = assembly;

            var instance = Activator.CreateInstance(versionModel.VersionType);

            Assert.NotNull(instance);
        }

        [Fact]
        public void IdAndDateProperties_PresentAndWorking()
        {
            var versionModel = _versionAssemblyBuilder.Add(_entityModel, new IVersionMetadataProvider[0]);
            var assembly = _versionAssemblyBuilder.Build();
            versionModel.Assembly = assembly;

            var instance = Activator.CreateInstance(versionModel.VersionType);

            // Id
            var idProperty = versionModel.VersionType.GetRuntimeProperty(nameof(IVersionEntity<int>.Id));
            Assert.NotNull(idProperty);
            Assert.Equal(typeof(long), idProperty.PropertyType);

            var idColumnAttribute = idProperty.GetCustomAttribute<ColumnAttribute>();
            Assert.NotNull(idColumnAttribute);
            Assert.Equal("Id", idColumnAttribute.Name);

            var idValue = 10L;
            idProperty.SetValue(instance, idValue);
            Assert.Equal(idValue, idProperty.GetValue(instance));

            // Date
            var dateProperty = versionModel.VersionType.GetRuntimeProperty(nameof(IVersionEntity<int>.Date));
            Assert.NotNull(dateProperty);
            Assert.Equal(typeof(DateTime), dateProperty.PropertyType);

            var dateColumnAttribute = dateProperty.GetCustomAttribute<ColumnAttribute>();
            Assert.NotNull(dateColumnAttribute);
            Assert.Equal("Date", dateColumnAttribute.Name);

            var dateValue = DateTime.UtcNow;
            dateProperty.SetValue(instance, dateValue);
            Assert.Equal(dateValue, dateProperty.GetValue(instance));
        }
        [Fact]
        public void PropertyForEachEntityProperty_PresentAndWorking()
        {
            var versionModel = _versionAssemblyBuilder.Add(_entityModel, new IVersionMetadataProvider[0]);
            var assembly = _versionAssemblyBuilder.Build();
            versionModel.Assembly = assembly;

            var instance = Activator.CreateInstance(versionModel.VersionType);

            // A
            Assert.True(versionModel.TryFindMapping(typeof(Entity).GetRuntimeProperty(nameof(Entity.A)), out var property));
            Assert.NotNull(property);
            Assert.Equal(typeof(int), property.PropertyType);

            var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
            Assert.NotNull(columnAttribute);
            Assert.EndsWith("_" + nameof(Entity.A), columnAttribute.Name);

            var value = 15;
            property.SetValue(instance, value);
            Assert.Equal(value, property.GetValue(instance));
        }

        [Fact]
        public void EntityCopyConstructor_PropertiesAreCopied()
        {
            var versionModel = _versionAssemblyBuilder.Add(_entityModel, new IVersionMetadataProvider[0]);
            var assembly = _versionAssemblyBuilder.Build();
            versionModel.Assembly = assembly;

            var value = 15;
            var entity = new Entity { A = value };

            var instance = Activator.CreateInstance(versionModel.VersionType, entity);

            var dateProperty = versionModel.VersionType.GetRuntimeProperty(nameof(IVersionEntity<int>.Date));
            Assert.InRange((DateTime)dateProperty.GetValue(instance), DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(1));

            Assert.True(versionModel.TryFindMapping(typeof(Entity).GetRuntimeProperty(nameof(Entity.A)), out var aProperty));
            Assert.Equal(value, aProperty.GetValue(instance));
        }

        [Fact]
        public void VersionEntityInterface_Implemented()
        {
            var versionModel = _versionAssemblyBuilder.Add(_entityModel, new IVersionMetadataProvider[0]);
            var assembly = _versionAssemblyBuilder.Build();
            versionModel.Assembly = assembly;

            Assert.True(typeof(IVersionEntity<Entity>).IsAssignableFrom(versionModel.VersionType));
        }

        [Fact]
        public void VersionEntityInterface_ToEntity_Works()
        {
            var versionModel = _versionAssemblyBuilder.Add(_entityModel, new IVersionMetadataProvider[0]);
            var assembly = _versionAssemblyBuilder.Build();
            versionModel.Assembly = assembly;

            var aValue = 15;
            var entity = new Entity { A = aValue };

            var instance = Activator.CreateInstance(versionModel.VersionType, entity);

            var toEntity = typeof(IVersionEntity<Entity>).GetRuntimeMethod(nameof(IVersionEntity<Entity>.ToEntity), Type.EmptyTypes);
            Assert.NotNull(toEntity);

            var result = toEntity.Invoke(instance, new object[0]);
            var entityResult = Assert.IsType<Entity>(result);

            Assert.Equal(aValue, entityResult.A);
        }
    }
}

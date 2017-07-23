using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Mono.Cecil;
using Neleus.LambdaCompare;
using Xunit;
using PropertyAttributes = Mono.Cecil.PropertyAttributes;
using TypeAttributes = Mono.Cecil.TypeAttributes;

namespace Orbital.Versioning.Tests
{
    public class VersionExpressionMapperTests
    {
        public interface IEntity
        {
            int BaseA { get; set; }
        }
        public class Entity : IEntity
        {
            public int A { get; set; }
            public int BaseA { get; set; }
        }
        public class Version
        {
            public int B { get; set; }
        }
        
        [Fact]
        public void Lambda_NoParameters_DoesNotChange()
        {
            Expression<Func<int>> expr = () => 1;

            var model = CreateModel();
            var mapper = new VersionExpressionMapper(model);

            var result = mapper.Visit(expr);

            Assert.True(Lambda.ExpressionsEqual(expr, result));
        }

        [Fact]
        public void NoEntityParameters_DoesNotChange()
        {
            Expression<Func<int, int>> expr = x => x + 1;

            var model = CreateModel();
            var mapper = new VersionExpressionMapper(model);

            var result = mapper.Visit(expr);

            Assert.True(Lambda.ExpressionsEqual(expr, result));
        }

        [Fact]
        public void SingleEntityParameter_MapsToVersionParameter()
        {
            Expression<Func<Entity, int>> input = x => x.A + 1;
            Expression<Func<Version, int>> expected = x => x.B + 1;

            var model = CreateModel(new Dictionary<string, string>
            {
                {nameof(Entity.A), nameof(Version.B)}
            });
            var mapper = new VersionExpressionMapper(model);

            var result = mapper.Visit(input);

            Assert.True(Lambda.ExpressionsEqual(expected, result));
        }

        [Fact]
        public void SingleEntityParameterOnInterface_MapsToVersionParameter()
        {
            Expression<Func<IEntity, int>> input = x => x.BaseA + 1;
            Expression<Func<Version, int>> expected = x => x.B + 1;

            var model = CreateModel(new Dictionary<string, string>
            {
                {nameof(IEntity.BaseA), nameof(Version.B)}
            });
            var mapper = new VersionExpressionMapper(model);

            var result = mapper.Visit(input);

            Assert.True(Lambda.ExpressionsEqual(expected, result));
        }
        [Fact]
        public void SingleEntityParameterOnCastedInterface_MapsToVersionParameter()
        {
            Expression<Func<Entity, int>> input = x => ((IEntity)x).BaseA + 1;
            Expression<Func<Version, int>> expected = x => x.B + 1;

            var model = CreateModel(new Dictionary<string, string>
            {
                {nameof(IEntity.BaseA), nameof(Version.B)}
            });
            var mapper = new VersionExpressionMapper(model);

            var result = mapper.Visit(input);

            Assert.True(Lambda.ExpressionsEqual(expected, result));
        }

        [Fact]
        public void SingleEntityParameter_NoMapping_ThrowsException()
        {
            Expression<Func<Entity, int>> input = x => x.A + 1;
            var model = CreateModel();
            var mapper = new VersionExpressionMapper(model);

            Assert.Throws<ArgumentException>(() => mapper.Visit(input));
        }

        [Fact]
        public void MultipleEntityParameters_MapsToMultipleVersionParameters()
        {
            Expression<Func<Entity, Entity, int>> input = (x, y) => x.A + y.A;
            Expression<Func<Version, Version, int>> expected = (x, y) => x.B + y.B;

            var model = CreateModel(new Dictionary<string, string>
            {
                {nameof(Entity.A), nameof(Version.B)}
            });
            var mapper = new VersionExpressionMapper(model);

            var result = mapper.Visit(input);

            Assert.True(Lambda.ExpressionsEqual(expected, result));
        }
        
        [Fact]
        public void VersionParameter_DoesNotChange()
        {
            Expression<Func<Version, int>> expr = x => x.B + 1;

            var model = CreateModel();
            var mapper = new VersionExpressionMapper(model);

            var result = mapper.Visit(expr);

            Assert.True(Lambda.ExpressionsEqual(expr, result));
        }

        private VersionModel CreateModel()
        {
            return CreateModel(new Dictionary<PropertyInfo, PropertyDefinition>());
        }

        private VersionModel CreateModel(IReadOnlyDictionary<string, string> mappings)
        {
            return CreateModel(mappings.ToDictionary(
                x => typeof(Entity).GetRuntimeProperty(x.Key),
                x => new PropertyDefinition(x.Value, PropertyAttributes.None, new TypeDefinition("", "", TypeAttributes.NotPublic))
            ));
        }

        private VersionModel CreateModel(IReadOnlyDictionary<PropertyInfo, PropertyDefinition> mappings)
        {
            var entityType = typeof(Entity);
            var versionType = typeof(Version);

            var versionReference = new TypeReference("", versionType.FullName, null, null);

            var entityModel = new EntityModel(entityType, null);

            return new VersionModel(null, versionReference, null, null, entityModel, mappings)
            {
                Assembly = versionType.GetTypeInfo().Assembly,
            };
        }
    }
}

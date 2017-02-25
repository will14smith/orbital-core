using System.Linq;
using GraphQL.Types;
using Orbital.Models.Domain;
using Orbital.Schema.Rounds;
using Xunit;

namespace Orbital.Schema.Tests.Rounds
{
    public class RoundTargetTypeTests
    {
        private static readonly RoundTarget WA18Target = RoundTypeTests.WA18.Targets.First();

        [Fact]
        public void TestFields()
        {
            var type = new RoundTargetType();
            var resolver = type.Fields.ToList();

            Assert.Equal(
              new[] { "Id", "ScoringType", "Distance", "FaceSize", "ArrowCount" }.OrderBy(x => x),
              resolver.Select(x => x.Name).OrderBy(x => x)
            );
        }

        [Fact]
        public void TestGetId()
        {
            var target = WA18Target;
            var type = new RoundTargetType();

            var resolver = type.Fields.First(x => x.Name == "Id").Resolver;
            var value = resolver.Resolve(new ResolveFieldContext { Source = target });
            Assert.Equal(target.Id, value);
        }

        [Fact]
        public void TestGetScoringType()
        {
            var target = WA18Target;
            var type = new RoundTargetType();

            var resolver = type.Fields.First(x => x.Name == "ScoringType").Resolver;
            var value = resolver.Resolve(new ResolveFieldContext { Source = target });
            Assert.Equal(target.ScoringType, value);
        }

        [Fact]
        public void TestGetDistance()
        {
            var target = WA18Target;
            var type = new RoundTargetType();

            var resolver = type.Fields.First(x => x.Name == "Distance").Resolver;
            var value = resolver.Resolve(new ResolveFieldContext { Source = target });
            Assert.Equal(target.Distance, value);
        }

        [Fact]
        public void TestGetFaceSize()
        {
            var target = WA18Target;
            var type = new RoundTargetType();

            var resolver = type.Fields.First(x => x.Name == "FaceSize").Resolver;
            var value = resolver.Resolve(new ResolveFieldContext { Source = target });
            Assert.Equal(target.FaceSize, value);
        }

        [Fact]
        public void TestGetArrowCount()
        {
            var target = WA18Target;
            var type = new RoundTargetType();

            var resolver = type.Fields.First(x => x.Name == "ArrowCount").Resolver;
            var value = resolver.Resolve(new ResolveFieldContext { Source = target });
            Assert.Equal(target.ArrowCount, value);
        }
    }
}

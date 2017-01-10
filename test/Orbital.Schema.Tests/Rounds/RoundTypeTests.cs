using System.Linq;
using GraphQL.Types;
using Orbital.Models.Domain;
using Orbital.Schema.Rounds;
using Xunit;

namespace Orbital.Schema.Tests.Rounds
{
    public class RoundTypeTests
    {
        internal static readonly Round WA18 = new Round(1, 2, "World Archery", "WA18", true, new[] { new RoundTarget(3, ScoringType.FiveZone, new Length(18, LengthUnit.Meters), new Length(40, LengthUnit.Centimeters), 60) });

        [Fact]
        public void TestFields()
        {
            var type = new RoundType();
            var resolver = type.Fields.ToList();

            Assert.Equal(
              new[] { "id", "variantOfId", "category", "name", "indoor", "targets" }.OrderBy(x => x),
              resolver.Select(x => x.Name).OrderBy(x => x)
            );
        }

        [Fact]
        public void TestGetId()
        {
            var round = WA18;
            var type = new RoundType();

            var resolver = type.Fields.First(x => x.Name == "id").Resolver;
            var value = resolver.Resolve(new ResolveFieldContext { Source = round });
            Assert.Equal(round.Id, value);
        }

        [Fact]
        public void TestGetVariantOfId()
        {
            var round = WA18;
            var type = new RoundType();

            var resolver = type.Fields.First(x => x.Name == "variantOfId").Resolver;
            var value = resolver.Resolve(new ResolveFieldContext { Source = round });
            Assert.Equal(round.VariantOfId, value);
        }

        [Fact]
        public void TestGetCategory()
        {
            var round = WA18;
            var type = new RoundType();

            var resolver = type.Fields.First(x => x.Name == "category").Resolver;
            var value = resolver.Resolve(new ResolveFieldContext { Source = round });
            Assert.Equal(round.Category, value);
        }

        [Fact]
        public void TestGetName()
        {
            var round = WA18;
            var type = new RoundType();

            var resolver = type.Fields.First(x => x.Name == "name").Resolver;
            var value = resolver.Resolve(new ResolveFieldContext { Source = round });
            Assert.Equal(round.Name, value);
        }

        [Fact]
        public void TestGetIndoor()
        {
            var round = WA18;
            var type = new RoundType();

            var resolver = type.Fields.First(x => x.Name == "indoor").Resolver;
            var value = resolver.Resolve(new ResolveFieldContext { Source = round });
            Assert.Equal(round.Indoor, value);
        }

        [Fact]
        public void TestGetTargets()
        {
            var round = WA18;
            var type = new RoundType();

            var resolver = type.Fields.First(x => x.Name == "targets").Resolver;
            var value = resolver.Resolve(new ResolveFieldContext { Source = round });
            Assert.Equal(round.Targets, value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Types;
using Moq;
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
              new[] { "id", "variantOfId", "category", "name", "indoor", "targets", "parent", "variants" }.OrderBy(x => x),
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

        [Fact]
        public void TestGetParent()
        {
            var round = WA18;
            var type = new RoundType();

            var resolver = type.Fields.First(x => x.Name == "parent").Resolver;

            // NOTE: this is returning on the VariantId, NOT the id - querying the orignal id will return null
            var roundService = Mock.Of<IRoundService>(x => x.GetById(round.VariantOfId.Value) == round);
            var userContext = Mock.Of<IUserContext>(x => x.ResolveService<IRoundService>() == roundService);

            var value = (Round)resolver.Resolve(new ResolveFieldContext { Source = round, UserContext = userContext });
            Assert.Equal(round.Id, value.Id);
        }
        [Fact]
        public void TestGetParent_Null()
        {
            var round = WA18;
            var type = new RoundType();

            var resolver = type.Fields.First(x => x.Name == "parent").Resolver;

            var roundService = Mock.Of<IRoundService>();
            var userContext = Mock.Of<IUserContext>(x => x.ResolveService<IRoundService>() == roundService);

            var value = (Round)resolver.Resolve(new ResolveFieldContext { Source = round, UserContext = userContext });
            Assert.Null(value);
        }

        [Fact]
        public void TestGetVariants()
        {
            var round = WA18;
            var type = new RoundType();

            var resolver = type.Fields.First(x => x.Name == "variants").Resolver;
            var roundService = Mock.Of<IRoundService>(x => x.GetVariants(round.Id) == new[] { round, round });
            var userContext = Mock.Of<IUserContext>(x => x.ResolveService<IRoundService>() == roundService);

            var value = (IReadOnlyCollection<Round>)resolver.Resolve(new ResolveFieldContext { Source = round, UserContext = userContext });
            Assert.Equal(2, value.Count);
        }
        [Fact]
        public void TestGetVariants_Empty()
        {
            var round = WA18;
            var type = new RoundType();

            var resolver = type.Fields.First(x => x.Name == "variants").Resolver;
            var roundService = Mock.Of<IRoundService>(x => x.GetVariants(round.Id) == new Round[0]);
            var userContext = Mock.Of<IUserContext>(x => x.ResolveService<IRoundService>() == roundService);

            var value = (IReadOnlyCollection<Round>)resolver.Resolve(new ResolveFieldContext { Source = round, UserContext = userContext });
            Assert.Equal(0, value.Count);
        }
    }
}

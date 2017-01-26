using System.Collections.Generic;
using System.Linq;
using GraphQL.Types;
using Moq;
using Orbital.Models.Domain;
using Orbital.Schema.Rounds;
using Xunit;

namespace Orbital.Schema.Tests.Rounds
{
    public class RoundMutationsTests
    {
        private readonly ObjectGraphType _mutations;

        public RoundMutationsTests()
        {
            _mutations = new ObjectGraphType();
            RoundMutations.AddToRoot(_mutations);
        }

        [Fact]
        public void TestAddToRoot()
        {
            Assert.Equal(
                new[] { "addRound", "updateRound" }.OrderBy(x => x),
                _mutations.Fields.Select(x => x.Name).OrderBy(x => x)
            );
        }

        [Fact]
        public void TestAddRoundResolve()
        {
            var round = new Round(0, RoundTypeTests.WA18);
            var input = new Dictionary<string, object>
            {
                {"variantOfId", round.VariantOfId},
                {"category", round.Category},
                {"name", round.Name},
                {"indoor", round.Indoor},

                {
                    "targets", round.Targets.Select(x => new Dictionary<string, object>
                    {
                        {"scoringType", x.ScoringType},
                        {"distance", new Dictionary<string, object> {{"value", x.Distance.Value}, {"unit", x.Distance.Unit}}},
                        {"faceSize", new Dictionary<string, object> {{"value", x.FaceSize.Value}, {"unit", x.FaceSize.Unit}}},
                        {"arrowCount", x.ArrowCount}
                    }).ToArray()
                }
            };

            var personServiceMock = new Mock<IRoundService>();
            personServiceMock.Setup(x => x.Add(It.IsAny<Round>())).Returns<Round>(x => new Round(1, x)).Verifiable();
            var userContext = Mock.Of<IUserContext>(x => x.ResolveService<IRoundService>() == personServiceMock.Object);

            var field = _mutations.Fields.First(x => x.Name == "addRound");
            var result = field.Resolver.Resolve(new ResolveFieldContext
            {
                Arguments = new Dictionary<string, object> { { "round", input } },
                UserContext = userContext
            });

            Assert.Equal(new Round(1, round), (Round)result, new Round.EqualWithoutId());
            personServiceMock.Verify();
        }

        [Fact]
        public void TestUpdateRoundResolve()
        {
            var round = RoundTypeTests.WA18;
            var input = new Dictionary<string, object>
            {
                {"variantOfId", round.VariantOfId},
                {"category", round.Category},
                {"name", round.Name},
                {"indoor", round.Indoor},

                {
                    "targets", round.Targets.Select(x => new Dictionary<string, object>
                    {
                        {"scoringType", x.ScoringType},
                        {"distance", new Dictionary<string, object> {{"value", x.Distance.Value}, {"unit", x.Distance.Unit}}},
                        {"faceSize", new Dictionary<string, object> {{"value", x.FaceSize.Value}, {"unit", x.FaceSize.Unit}}},
                        {"arrowCount", x.ArrowCount}
                    }).ToArray()
                }
            };

            var personServiceMock = new Mock<IRoundService>();
            personServiceMock.Setup(x => x.Update(round.Id, It.IsAny<Round>())).Returns<int, Round>((id, x) => new Round(id, x)).Verifiable();
            var userContext = Mock.Of<IUserContext>(x => x.ResolveService<IRoundService>() == personServiceMock.Object);

            var field = _mutations.Fields.First(x => x.Name == "updateRound");
            var result = field.Resolver.Resolve(new ResolveFieldContext
            {
                Arguments = new Dictionary<string, object> { { "id", round.Id }, { "round", input } },
                UserContext = userContext
            });

            Assert.Equal(round, (Round)result, new Round.EqualWithoutId());
            personServiceMock.Verify();
        }
    }
}
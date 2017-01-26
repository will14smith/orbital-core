using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Types;
using Moq;
using Orbital.Models.Domain;
using Orbital.Schema.Clubs;
using Orbital.Schema.Competitions;
using Xunit;

namespace Orbital.Schema.Tests.Competitions
{
    public class CompetitionMutationTests
    {
        private static readonly DateTime Start = new DateTime(2017, 1, 1);
        private static readonly DateTime End = new DateTime(2017, 1, 2);

        private readonly ObjectGraphType _mutations;

        public CompetitionMutationTests()
        {
            _mutations = new ObjectGraphType();
            CompetitionMutations.AddToRoot(_mutations);
        }

        [Fact]
        public void TestAddToRoot()
        {
            Assert.Equal(
              new[] { "addCompetition", "updateCompetition" }.OrderBy(x => x),
              _mutations.Fields.Select(x => x.Name).OrderBy(x => x)
            );
        }

        [Fact]
        public void TestAddClubResolve()
        {
            var competition = new Competition(0, "CompetitionName", Start, End, new[] { 2,3,4 });
            var input = new Dictionary<string, object>
            {
                { "name", competition.Name },
                { "start", competition.Start },
                { "end", competition.End },
                { "rounds", competition.Rounds },
            };

            var competitionServiceMock = new Mock<ICompetitionService>();
            competitionServiceMock.Setup(x => x.Add(It.IsAny<Competition>())).Returns<Competition>(x => new Competition(1, x)).Verifiable();
            var userContext = Mock.Of<IUserContext>(x => x.ResolveService<ICompetitionService>() == competitionServiceMock.Object);

            var field = _mutations.Fields.First(x => x.Name == "addCompetition");
            var result = field.Resolver.Resolve(new ResolveFieldContext
            {
                Arguments = new Dictionary<string, object> { { "competition", input } },
                UserContext = userContext
            });

            Assert.Equal(new Competition(1, competition), result);
            competitionServiceMock.Verify();
        }

        [Fact]
        public void TestUpdateClubResolve()
        {
            var competition = new Competition(1, "CompetitionName", Start, End, new[] { 2, 3, 4 });
            var input = new Dictionary<string, object>
            {
                { "name", competition.Name },
                { "start", competition.Start },
                { "end", competition.End },
                { "rounds", competition.Rounds },
            };

            var competitionServiceMock = new Mock<ICompetitionService>();
            competitionServiceMock.Setup(x => x.Update(competition.Id, It.IsAny<Competition>())).Returns<int, Competition>((id, x) => new Competition(id, x)).Verifiable();
            var userContext = Mock.Of<IUserContext>(x => x.ResolveService<ICompetitionService>() == competitionServiceMock.Object);

            var field = _mutations.Fields.First(x => x.Name == "updateCompetition");
            var result = field.Resolver.Resolve(new ResolveFieldContext
            {
                Arguments = new Dictionary<string, object> { { "id", competition.Id }, { "competition", input } },
                UserContext = userContext
            });

            Assert.Equal(competition, result);
            competitionServiceMock.Verify();
        }
    }
}

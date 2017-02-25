using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Types;
using Moq;
using Orbital.Models.Domain;
using Orbital.Models.Services;
using Orbital.Schema.Competitions;
using Orbital.Schema.Rounds;
using Xunit;

namespace Orbital.Schema.Tests.Competitions
{
    public class CompetitionTypeTests
    {
        private static readonly DateTime Start = new DateTime(2017, 1, 1);
        private static readonly DateTime End = new DateTime(2017, 1, 2);

        [Fact]
        public void TestFields()
        {
            var type = new CompetitionType();
            var resolver = type.Fields.ToList();

            Assert.Equal(
              new[] { "Id", "Name", "Start", "End", "Rounds" }.OrderBy(x => x),
              resolver.Select(x => x.Name).OrderBy(x => x)
            );
        }

        [Fact]
        public void TestGetId()
        {
            var competition = new Competition(1, "CompetitionName", Start, End, new int[0]);
            var type = new CompetitionType();

            var resolver = type.Fields.First(x => x.Name == "Id").Resolver;
            var value = resolver.Resolve(new ResolveFieldContext { Source = competition });
            Assert.Equal(competition.Id, value);
        }

        [Fact]
        public void TestGetName()
        {
            var competition = new Competition(1, "CompetitionName", Start, End, new int[0]);
            var type = new CompetitionType();

            var resolver = type.Fields.First(x => x.Name == "Name").Resolver;
            var value = resolver.Resolve(new ResolveFieldContext { Source = competition });
            Assert.Equal(competition.Name, value);
        }

        [Fact]
        public void TestGetStart()
        {
            var competition = new Competition(1, "CompetitionName", Start, End, new int[0]);
            var type = new CompetitionType();

            var resolver = type.Fields.First(x => x.Name == "Start").Resolver;
            var value = resolver.Resolve(new ResolveFieldContext { Source = competition });
            Assert.Equal(competition.Start, value);
        }

        [Fact]
        public void TestGetEnd()
        {
            var competition = new Competition(1, "CompetitionName", Start, End, new int[0]);
            var type = new CompetitionType();

            var resolver = type.Fields.First(x => x.Name == "End").Resolver;
            var value = resolver.Resolve(new ResolveFieldContext { Source = competition });
            Assert.Equal(competition.End, value);
        }

        [Fact]
        public void TestGetPeople()
        {
            var competition = new Competition(1, "CompetitionName", Start, End, new int[0]);
            var round = Mock.Of<Round>();

            var roundService = Mock.Of<IRoundService>(x => x.GetByCompetition(competition) == new[] { round });
            var userContext = Mock.Of<IUserContext>(x => x.ResolveService<IRoundService>() == roundService);

            var type = new CompetitionType();
            var resolver = type.Fields.First(x => x.Name == "Rounds").Resolver;
            var value = resolver.Resolve(new ResolveFieldContext
            {
                Source = competition,
                UserContext = userContext,
            });

            Assert.IsAssignableFrom<IEnumerable<Round>>(value);
            var list = value as IEnumerable<Round>;
            Assert.Single(list, round);
        }
    }
}
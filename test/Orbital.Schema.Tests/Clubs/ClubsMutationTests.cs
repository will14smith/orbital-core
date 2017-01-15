using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Types;
using Moq;
using Orbital.Models.Domain;
using Orbital.Schema.Clubs;
using Xunit;

namespace Orbital.Schema.Tests.Clubs
{
    public class ClubsMutationTests
    {
        private readonly ObjectGraphType _mutations;

        public ClubsMutationTests()
        {
            _mutations = new ObjectGraphType();
            ClubMutations.AddToRoot(_mutations);
        }

        [Fact]
        public void TestAddToRoot()
        {
            Assert.Equal(
              new[] { "addClub", "updateClub" }.OrderBy(x => x),
              _mutations.Fields.Select(x => x.Name).OrderBy(x => x)
            );
        }

        [Fact]
        public void TestAddClubResolve()
        {
            var club = new Club(0, "ClubName");
            var input = new Dictionary<string, object> { { "name", club.Name } };

            var clubServiceMock = new Mock<IClubService>();
            clubServiceMock.Setup(x => x.Add(It.IsAny<Club>())).Returns<Club>(x => new Club(1, x.Name)).Verifiable();
            var userContext = Mock.Of<IUserContext>(x => x.ResolveService<IClubService>() == clubServiceMock.Object);

            var field = _mutations.Fields.First(x => x.Name == "addClub");
            var result = field.Resolver.Resolve(new ResolveFieldContext
            {
                Arguments = new Dictionary<string, object> { { "club", input } },
                UserContext = userContext
            });

            Assert.Equal(new Club(1, club.Name), result);
            clubServiceMock.Verify();
        }

        [Fact]
        public void TestUpdateClubResolve()
        {
            var club = new Club(1, "ClubName");
            var input = new Dictionary<string, object> { { "name", club.Name } };

            var clubServiceMock = new Mock<IClubService>();
            clubServiceMock.Setup(x => x.Update(club.Id, It.IsAny<Club>())).Returns<int, Club>((id, x) => new Club(id, x.Name)).Verifiable();
            var userContext = Mock.Of<IUserContext>(x => x.ResolveService<IClubService>() == clubServiceMock.Object);

            var field = _mutations.Fields.First(x => x.Name == "updateClub");
            var result = field.Resolver.Resolve(new ResolveFieldContext
            {
                Arguments = new Dictionary<string, object> { { "id", club.Id }, { "club", input } },
                UserContext = userContext
            });

            Assert.Equal(club, result);
            clubServiceMock.Verify();
        }
    }
}

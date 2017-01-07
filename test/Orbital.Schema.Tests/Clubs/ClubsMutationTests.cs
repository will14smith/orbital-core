using System;
using System.Linq;
using GraphQL.Types;
using Orbital.Schema.Clubs;
using Xunit;

namespace Orbital.Schema.Tests.Clubs
{
    public class ClubsMutationTests
    {
        [Fact]
        public void TestAddToRoot()
        {
            var type = new ObjectGraphType();
            ClubMutations.AddToRoot(type);

            Assert.Equal(
              new[] { "addClub", "updateClub" }.OrderBy(x => x),
              type.Fields.Select(x => x.Name).OrderBy(x => x)
            );
        }

        [Fact]
        public void TestAddClubResolve()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestUpdateClubResolve()
        {
            throw new NotImplementedException();
        }
    }
}

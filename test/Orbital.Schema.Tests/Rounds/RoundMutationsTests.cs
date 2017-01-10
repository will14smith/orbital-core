using System;
using System.Linq;
using GraphQL.Types;
using Orbital.Schema.Clubs;
using Orbital.Schema.Rounds;
using Xunit;

namespace Orbital.Schema.Tests.Rounds
{
    public class RoundMutationsTests
    {
        [Fact]
        public void TestAddToRoot()
        {
            var type = new ObjectGraphType();
            RoundMutations.AddToRoot(type);

            Assert.Equal(
              new[] { "addRound", "updateRound" }.OrderBy(x => x),
              type.Fields.Select(x => x.Name).OrderBy(x => x)
            );
        }

        [Fact]
        public void TestAddRoundResolve()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestUpdateRoundResolve()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Linq;
using GraphQL.Types;
using Orbital.Schema.People;
using Xunit;

namespace Orbital.Schema.Tests.People
{
    public class PersonMutationsTests
    {
        [Fact]
        public void TestAddToRoot()
        {
            var type = new ObjectGraphType();
            PersonMutations.AddToRoot(type);

            Assert.Equal(
              new[] { "addPerson", "updatePerson" }.OrderBy(x => x),
              type.Fields.Select(x => x.Name).OrderBy(x => x)
            );
        }

        [Fact]
        public void TestAddPersonResolve()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestUpdatePersonResolve()
        {
            throw new NotImplementedException();
        }
    }
}

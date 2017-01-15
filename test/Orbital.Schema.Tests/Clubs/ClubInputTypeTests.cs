using System.Linq;
using Orbital.Schema.Clubs;
using Xunit;

namespace Orbital.Schema.Tests.Clubs
{
    public class ClubInputTypeTests
    {
        [Fact]
        public void TestFields()
        {
            var type = new ClubInputType();

            Assert.Equal(
                new[] { "name" }.OrderBy(x => x),
                type.Fields.Select(x => x.Name).OrderBy(x => x)
            );
        }
    }
}

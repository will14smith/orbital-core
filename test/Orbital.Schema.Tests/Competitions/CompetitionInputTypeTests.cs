using System.Linq;
using Orbital.Schema.Competitions;
using Xunit;

namespace Orbital.Schema.Tests.Competitions
{
    public class CompetitionInputTypeTests
    {
        [Fact]
        public void TestFields()
        {
            var type = new CompetitionInputType();

            Assert.Equal(
                new[] { "name", "start", "end", "rounds" }.OrderBy(x => x),
                type.Fields.Select(x => x.Name).OrderBy(x => x)
            );
        }
    }
}

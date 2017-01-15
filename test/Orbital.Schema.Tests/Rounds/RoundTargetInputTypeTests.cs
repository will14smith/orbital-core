using System.Linq;
using Orbital.Schema.Rounds;
using Xunit;

namespace Orbital.Schema.Tests.Rounds
{
    public class RoundTargetInputTypeTests
    {
        [Fact]
        public void TestFields()
        {
            var type = new RoundTargetInputType();
            var resolver = type.Fields.ToList();

            Assert.Equal(
              new[] { "scoringType", "distance", "faceSize", "arrowCount" }.OrderBy(x => x),
              resolver.Select(x => x.Name).OrderBy(x => x)
            );
        }
    }
}

using System.Linq;
using Orbital.Schema.Rounds;
using Xunit;

namespace Orbital.Schema.Tests.Rounds
{
    public class RoundInputTypeTests
    {
        [Fact]
        public void TestFields()
        {
            var type = new RoundInputType();
            var resolver = type.Fields.ToList();

            Assert.Equal(
              new[] { "variantOfId", "category", "name", "indoor", "targets" }.OrderBy(x => x),
              resolver.Select(x => x.Name).OrderBy(x => x)
            );
        }
    }
}

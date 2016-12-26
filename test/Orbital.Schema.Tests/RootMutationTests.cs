using System.Linq;
using Xunit;

namespace Orbital.Schema.Tests
{
  public class RootMutationTests
  {
    [Fact]
    public void TestFields()
    {
      var type = new RootMutation();
      var resolver = type.Fields.ToList();

      Assert.Equal(
        new string[] { }.OrderBy(x => x),
        resolver.Select(x => x.Name).OrderBy(x => x)
      );
    }
  }
}
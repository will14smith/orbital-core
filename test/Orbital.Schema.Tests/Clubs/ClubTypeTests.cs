using System.Linq;
using GraphQL.Types;
using Orbital.Models.Domain;
using Orbital.Schema.Clubs;
using Xunit;

namespace Orbital.Schema.Tests.Clubs
{
  public class ClubTypeTests
  {
    [Fact]
    public void TestGetId()
    {
      var club = new Club(1, "Hello");
      var type = new ClubType();

      var resolver = type.Fields.First(x => x.Name == "id").Resolver;
      var value = resolver.Resolve(new ResolveFieldContext { Source = club });
      Assert.Equal(club.Id, value);
    }

    [Fact]
    public void TestGetName()
    {
      var club = new Club(1, "Hello");
      var type = new ClubType();

      var resolver = type.Fields.First(x => x.Name == "name").Resolver;
      var value = resolver.Resolve(new ResolveFieldContext { Source = club });
      Assert.Equal(club.Name, value);
    }
  }
}
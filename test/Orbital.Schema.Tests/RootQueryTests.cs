using System.Collections.Generic;
using System.Linq;
using GraphQL.Types;
using Moq;
using Orbital.Models.Domain;
using Orbital.Schema.Clubs;
using Orbital.Schema.People;
using Xunit;

namespace Orbital.Schema.Tests
{
  public class RootQueryTests
  {
    [Fact]
    public void TestFields()
    {
      var type = new RootQuery();
      var resolver = type.Fields.ToList();

      Assert.Equal(
        new string[] { "clubs", "people" }.OrderBy(x => x),
        resolver.Select(x => x.Name).OrderBy(x => x)
      );
    }

    [Fact]
    public void TestGetClubs()
    {
      var club = new Club(1, "ClubName");

      var clubService = Mock.Of<IClubService>(x => x.GetRoot() == new[] { club });
      var userContext = Mock.Of<IUserContext>(x => x.ResolveService<IClubService>() == clubService);

      var type = new RootQuery();
      var resolver = type.Fields.First(x => x.Name == "clubs").Resolver;
      var value = resolver.Resolve(new ResolveFieldContext
      {
        UserContext = userContext,
      });

      Assert.IsAssignableFrom<IEnumerable<Club>>(value);
      var list = value as IEnumerable<Club>;
      Assert.Single(list, club);
    }

    [Fact]
    public void TestGetPeople()
    {
      var person = new Person(1, 2, "PersonName", Gender.Male);

      var personService = Mock.Of<IPersonService>(x => x.GetRoot() == new[] { person });
      var userContext = Mock.Of<IUserContext>(x => x.ResolveService<IPersonService>() == personService);

      var type = new RootQuery();
      var resolver = type.Fields.First(x => x.Name == "people").Resolver;
      var value = resolver.Resolve(new ResolveFieldContext
      {
        UserContext = userContext,
      });

      Assert.IsAssignableFrom<IEnumerable<Person>>(value);
      var list = value as IEnumerable<Person>;
      Assert.Single(list, person);
    }
  }
}
using System.Collections.Generic;
using System.Linq;
using GraphQL.Types;
using Moq;
using Orbital.Models.Domain;
using Orbital.Schema.Clubs;
using Orbital.Schema.People;
using Xunit;

namespace Orbital.Schema.Tests.Clubs
{
  public class ClubTypeTests
  {
    [Fact]
    public void TestFields()
    {
      var type = new ClubType();
      var resolver = type.Fields.ToList();

      Assert.Equal(
        new[] { "id", "name", "people" }.OrderBy(x => x),
        resolver.Select(x => x.Name).OrderBy(x => x)
      );
    }

    [Fact]
    public void TestGetId()
    {
      var club = new Club(1, "ClubName");
      var type = new ClubType();

      var resolver = type.Fields.First(x => x.Name == "id").Resolver;
      var value = resolver.Resolve(new ResolveFieldContext { Source = club });
      Assert.Equal(club.Id, value);
    }

    [Fact]
    public void TestGetName()
    {
      var club = new Club(1, "ClubName");
      var type = new ClubType();

      var resolver = type.Fields.First(x => x.Name == "name").Resolver;
      var value = resolver.Resolve(new ResolveFieldContext { Source = club });
      Assert.Equal(club.Name, value);
    }

    [Fact]
    public void TestGetPeople()
    {
      var club = new Club(1, "ClubName");
      var person = new Person(1, club.Id, "PersonName", Gender.Male);

      var personService = Mock.Of<IPersonService>(x => x.GetByClub(club) == new[] { person });
      var userContext = Mock.Of<IUserContext>(x => x.ResolveService<IPersonService>() == personService);

      var type = new ClubType();
      var resolver = type.Fields.First(x => x.Name == "people").Resolver;
      var value = resolver.Resolve(new ResolveFieldContext
      {
        Source = club,
        UserContext = userContext,
      });

      Assert.IsAssignableFrom<IEnumerable<Person>>(value);
      var list = value as IEnumerable<Person>;
      Assert.Single(list, person);
    }
  }
}
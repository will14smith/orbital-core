using System;
using System.Linq;
using GraphQL.Types;
using Moq;
using Orbital.Models.Domain;
using Orbital.Models.Services;
using Orbital.Schema.Clubs;
using Orbital.Schema.People;
using Xunit;

namespace Orbital.Schema.Tests.People
{
  public class PersonTypeTests
  {
    [Fact]
    public void TestFields()
    {
      var type = new PersonType();
      var resolver = type.Fields.ToList();

      Assert.Equal(
        new[] { "id", "club", "name", "gender", "bowstyle", "archeryGBNumber", "dateOfBirth", "dateStartedArchery" }.OrderBy(x => x),
        resolver.Select(x => x.Name).OrderBy(x => x)
      );
    }

    [Fact]
    public void TestGetId()
    {
      var person = new Person(1, 2, "PersonName", Gender.Male);
      var type = new PersonType();

      var resolver = type.Fields.First(x => x.Name == "id").Resolver;
      var value = resolver.Resolve(new ResolveFieldContext { Source = person });
      Assert.Equal(person.Id, value);
    }

    [Fact]
    public void TestGetClub()
    {
      var club = new Club(2, "ClubName");
      var person = new Person(1, club.Id, "PersonName", Gender.Male);

      var clubService = Mock.Of<IClubService>(x => x.GetById(club.Id) == club);
      var userContext = Mock.Of<IUserContext>(x => x.ResolveService<IClubService>() == clubService);

      var type = new PersonType();
      var resolver = type.Fields.First(x => x.Name == "club").Resolver;
      var value = resolver.Resolve(new ResolveFieldContext
      {
        Source = person,
        UserContext = userContext,
      });

      Assert.IsAssignableFrom<Club>(value);
      var resultClub = value as Club;
      Assert.Equal(club, resultClub);
    }

    [Fact]
    public void TestGetName()
    {
      var person = new Person(1, 2, "PersonName", Gender.Male);
      var type = new PersonType();

      var resolver = type.Fields.First(x => x.Name == "name").Resolver;
      var value = resolver.Resolve(new ResolveFieldContext { Source = person });
      Assert.Equal(person.Name, value);
    }

    [Fact]
    public void TestGetGender()
    {
      var person = new Person(1, 2, "PersonName", Gender.Female);
      var type = new PersonType();

      var resolver = type.Fields.First(x => x.Name == "gender").Resolver;
      var value = resolver.Resolve(new ResolveFieldContext { Source = person });
      Assert.Equal(person.Gender, value);
    }

    [Fact]
    public void TestGetBowstyle()
    {
      var person = new Person(1, 2, "PersonName", Gender.Female, bowstyle: Bowstyle.Longbow);
      var type = new PersonType();

      var resolver = type.Fields.First(x => x.Name == "bowstyle").Resolver;
      var value = resolver.Resolve(new ResolveFieldContext { Source = person });
      Assert.Equal(person.Bowstyle, value);
    }
    [Fact]
    public void TestGetBowstyle_empty()
    {
      var person = new Person(1, 2, "PersonName", Gender.Female);
      var type = new PersonType();

      var resolver = type.Fields.First(x => x.Name == "bowstyle").Resolver;
      var value = resolver.Resolve(new ResolveFieldContext { Source = person });
      Assert.Null(value);
    }

    [Fact]
    public void TestGetArcheryGBNumber()
    {
      var person = new Person(1, 2, "PersonName", Gender.Female, archeryGBNumber: "ArcheryGB");
      var type = new PersonType();

      var resolver = type.Fields.First(x => x.Name == "archeryGBNumber").Resolver;
      var value = resolver.Resolve(new ResolveFieldContext { Source = person });
      Assert.Equal(person.ArcheryGBNumber, value);
    }
    [Fact]
    public void TestGetArcheryGBNumber_empty()
    {
      var person = new Person(1, 2, "PersonName", Gender.Female);
      var type = new PersonType();

      var resolver = type.Fields.First(x => x.Name == "archeryGBNumber").Resolver;
      var value = resolver.Resolve(new ResolveFieldContext { Source = person });
      Assert.Equal(person.ArcheryGBNumber, value);
    }


    [Fact]
    public void TestGetDateOfBirth()
    {
      var person = new Person(1, 2, "PersonName", Gender.Female, dateOfBirth: new DateTime());
      var type = new PersonType();

      var resolver = type.Fields.First(x => x.Name == "dateOfBirth").Resolver;
      var value = resolver.Resolve(new ResolveFieldContext { Source = person });
      Assert.Equal(person.DateOfBirth, value);
    }
    [Fact]
    public void TestGetDateOfBirth_empty()
    {
      var person = new Person(1, 2, "PersonName", Gender.Female);
      var type = new PersonType();

      var resolver = type.Fields.First(x => x.Name == "dateOfBirth").Resolver;
      var value = resolver.Resolve(new ResolveFieldContext { Source = person });
      Assert.Equal(person.DateOfBirth, value);
    }


    [Fact]
    public void TestGetDateStartedArchery()
    {
      var person = new Person(1, 2, "PersonName", Gender.Female, dateStartedArchery: new DateTime());
      var type = new PersonType();

      var resolver = type.Fields.First(x => x.Name == "dateStartedArchery").Resolver;
      var value = resolver.Resolve(new ResolveFieldContext { Source = person });
      Assert.Equal(person.DateStartedArchery, value);
    }
    [Fact]
    public void TestGetDateStartedArchery_empty()
    {
      var person = new Person(1, 2, "PersonName", Gender.Female);
      var type = new PersonType();

      var resolver = type.Fields.First(x => x.Name == "dateStartedArchery").Resolver;
      var value = resolver.Resolve(new ResolveFieldContext { Source = person });
      Assert.Equal(person.DateStartedArchery, value);
    }
  }
}
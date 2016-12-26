using System.Linq;
using Orbital.Data.Repositories;
using Orbital.Models.Domain;
using Orbital.Schema.People;
using Xunit;

namespace Orbital.Schema.Tests.People
{
  public class PersonServiceImplTests
  {
    [Fact]
    public void TestGetRoot()
    {
      var person = new Person(1, 2, "PersonName", Gender.Male);
      var personRepository = InMemoryPersonRepository.New(person);

      var service = new PersonServiceImpl(personRepository);

      var result = service.GetRoot();
      Assert.Single(result, person);
    }

    [Fact]
    public void TestGetByClub()
    {
      var club = new Club(2, "ClubName");
      var person = new Person(1, 2, "PersonName", Gender.Male);
      var personRepository = InMemoryPersonRepository.New(person);

      var service = new PersonServiceImpl(personRepository);

      var result = service.GetByClub(club);
      Assert.Single(result, person);
    }

    [Fact]
    public void TestGetByClub_Different()
    {
      var club = new Club(1, "ClubName");
      var person = new Person(1, 2, "PersonName", Gender.Male);
      var personRepository = InMemoryPersonRepository.New(person);

      var service = new PersonServiceImpl(personRepository);

      var result = service.GetByClub(club);
      Assert.Empty(result);
    }
  }
}
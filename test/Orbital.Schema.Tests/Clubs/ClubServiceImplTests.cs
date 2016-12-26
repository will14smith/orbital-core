using System.Linq;
using Orbital.Data.Repositories;
using Orbital.Models.Domain;
using Orbital.Schema.Clubs;
using Xunit;

namespace Orbital.Schema.Tests.Clubs
{
  public class ClubServiceImplTests
  {
    [Fact]
    public void TestGetRoot()
    {
      var club = new Club(1, "Hello");
      var clubRepository = InMemoryClubRepository.New(club);

      var service = new ClubServiceImpl(clubRepository);

      var result = service.GetRoot();
      Assert.Single(result, club);
    }

    [Fact]
    public void TestGetById()
    {
      var club = new Club(1, "Hello");
      var clubRepository = InMemoryClubRepository.New(club);

      var service = new ClubServiceImpl(clubRepository);

      var result = service.GetById(club.Id);
      Assert.Equal(club, result);
    }
  }
}
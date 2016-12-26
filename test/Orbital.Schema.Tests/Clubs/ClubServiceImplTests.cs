using System.Linq;
using Moq;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;
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
      var clubRepository = Mock.Of<IClubRepository>(
        x => x.GetAll() == new[] { club }
      );

      var service = new ClubServiceImpl(clubRepository);

      var result = service.GetRoot();
      Assert.Equal(result.Count, 1);
      Assert.Equal(result.First(), club);
    }
  }
}
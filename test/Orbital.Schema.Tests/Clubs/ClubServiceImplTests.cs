using System;
using Moq;
using Orbital.Data.Repositories;
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

        [Fact]
        public void TestAdd()
        {
            var club = new Club(0, "Hello");
            var clubRepositoryMock = new Mock<IClubRepository>();
            clubRepositoryMock.Setup(x => x.Create(club)).Returns<Club>(x => new Club(1, x.Name)).Verifiable();

            var service = new ClubServiceImpl(clubRepositoryMock.Object);

            var result = service.Add(club);
            Assert.Equal(new Club(1, club.Name), result);

            clubRepositoryMock.Verify();
        }

        [Fact]
        public void TestUpdate()
        {
            var club = new Club(1, "Hello");
            var clubRepositoryMock = new Mock<IClubRepository>();
            clubRepositoryMock.Setup(x => x.Update(club)).Returns<Club>(x => x).Verifiable();

            var service = new ClubServiceImpl(clubRepositoryMock.Object);

            var result = service.Update(club.Id, club);
            Assert.Equal(club, result);

            clubRepositoryMock.Verify();
        }
    }
}
using System;
using Moq;
using Orbital.Data.Repositories;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;
using Xunit;

namespace Orbital.Services.Test
{
    public class CompetitionServiceImplTests
    {
        [Fact]
        public void TestGetRoot()
        {
            var competition = new Competition(1, "CompetitionName", new DateTime(2017, 1, 1), new DateTime(2017, 1, 2), new int[0]);
            var competitionRepository = InMemoryCompetitionRepository.New(competition);

            var service = new CompetitionServiceImpl(competitionRepository);

            var result = service.GetRoot();
            Assert.Single(result, competition);
        }

        [Fact]
        public void TestGetById()
        {
            var competition = new Competition(1, "CompetitionName", new DateTime(2017, 1, 1), new DateTime(2017, 1, 2), new int[0]);
            var competitionRepository = InMemoryCompetitionRepository.New(competition);

            var service = new CompetitionServiceImpl(competitionRepository);

            var result = service.GetById(competition.Id);
            Assert.Equal(1, result.Id);
            Assert.Equal(competition, result);
        }

        [Fact]
        public void TestAdd()
        {
            var competition = new Competition(0, "CompetitionName", new DateTime(2017, 1, 1), new DateTime(2017, 1, 2), new int[0]);
            var competitionRepositoryMock = new Mock<ICompetitionRepository>();
            competitionRepositoryMock.Setup(x => x.Create(competition)).Returns<Competition>(x => new Competition(1, competition)).Verifiable();

            var service = new CompetitionServiceImpl(competitionRepositoryMock.Object);

            var result = service.Add(competition);
            Assert.Equal(new Competition(1, competition), result);

            competitionRepositoryMock.Verify();
        }

        [Fact]
        public void TestUpdate()
        {
            var competition = new Competition(1, "CompetitionName", new DateTime(2017, 1, 1), new DateTime(2017, 1, 2), new int[0]);
            var competitionRepositoryMock = new Mock<ICompetitionRepository>();
            competitionRepositoryMock.Setup(x => x.Update(It.IsAny<Competition>())).Returns<Competition>(x => x).Verifiable();

            var service = new CompetitionServiceImpl(competitionRepositoryMock.Object);

            var result = service.Update(competition.Id, competition);
            Assert.Equal(competition, result);

            competitionRepositoryMock.Verify();
        }
    }
}
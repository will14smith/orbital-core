using Moq;
using Orbital.Data.Repositories;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;
using Orbital.Schema.Rounds;
using Xunit;

namespace Orbital.Schema.Tests.Rounds
{
    public class RoundServiceImplTests
    {
        [Fact]
        public void TestGetRoot()
        {
            var round = RoundTypeTests.WA18;
            var roundRepository = InMemoryRoundRepository.New(round);

            var service = new RoundServiceImpl(roundRepository);

            var result = service.GetRoot();
            Assert.Single(result, round);
        }

        [Fact]
        public void TestAdd()
        {
            var round = new Round(0, RoundTypeTests.WA18);
            var roundRepositoryMock = new Mock<IRoundRepository>();
            roundRepositoryMock.Setup(x => x.Create(round)).Returns<Round>(x => new Round(1, round)).Verifiable();

            var service = new RoundServiceImpl(roundRepositoryMock.Object);

            var result = service.Add(round);
            Assert.Equal(1, result.Id);
            Assert.Equal(round, result, new Round.EqualWithoutId());

            roundRepositoryMock.Verify();
        }

        [Fact]
        public void TestUpdate()
        {
            var round = RoundTypeTests.WA18;
            var roundRepositoryMock = new Mock<IRoundRepository>();
            roundRepositoryMock.Setup(x => x.Update(It.IsAny<Round>())).Returns<Round>(x => x).Verifiable();

            var service = new RoundServiceImpl(roundRepositoryMock.Object);

            var result = service.Update(round.Id, round);
            Assert.Equal(round.Id, result.Id);
            Assert.Equal(round, result, new Round.EqualWithoutId());

            roundRepositoryMock.Verify();
        }
    }
}
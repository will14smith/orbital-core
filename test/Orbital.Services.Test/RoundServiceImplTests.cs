using System;
using Moq;
using Orbital.Data.Repositories;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;
using Xunit;

namespace Orbital.Services.Test
{
    public class RoundServiceImplTests
    {
        internal static readonly Round WA18 = new Round(1, 2, "World Archery", "WA18", true, new[] { new RoundTarget(3, ScoringType.FiveZone, new Length(18, LengthUnit.Meters), new Length(40, LengthUnit.Centimeters), 60) });

        [Fact]
        public void TestGetRoot()
        {
            var round = WA18;
            var roundRepository = InMemoryRoundRepository.New(round);

            var service = new RoundServiceImpl(roundRepository);

            var result = service.GetRoot();
            Assert.Single(result, round);
        }

        [Fact]
        public void TestGetVariants()
        {
            var round1 = WA18;
            var round2 = new Round(round1.VariantOfId.Value, null, "A", "B", false, new RoundTarget[0]);
            var roundRepository = InMemoryRoundRepository.New(round1, round2);

            var service = new RoundServiceImpl(roundRepository);

            var result = service.GetVariants(round2.Id);
            Assert.Single(result, round1);
        }

        [Fact]
        public void TestGetByCompetition_Empty()
        {
            var competition = new Competition(1, "CompName", new DateTime(2017, 1, 1), new DateTime(2017, 1, 2), new int[0]);
            var roundRepository = InMemoryRoundRepository.New();

            var service = new RoundServiceImpl(roundRepository);

            var result = service.GetByCompetition(competition);
            Assert.Empty(result);
        }

        [Fact]
        public void TestGetByCompetition()
        {
            var round = WA18;
            var roundRepository = InMemoryRoundRepository.New(round);
            var competition = new Competition(1, "CompName", new DateTime(2017, 1, 1), new DateTime(2017, 1, 2), new[] { round.Id });

            var service = new RoundServiceImpl(roundRepository);

            var result = service.GetByCompetition(competition);
            Assert.Single(result, round);
        }

        [Fact]
        public void TestGetById()
        {
            var round = WA18;
            var roundRepository = InMemoryRoundRepository.New(round);

            var service = new RoundServiceImpl(roundRepository);

            var result = service.GetById(round.Id);
            Assert.Equal(1, result.Id);
            Assert.Equal(round, result, new Round.EqualWithoutId());
        }

        [Fact]
        public void TestAdd()
        {
            var round = new Round(0, WA18);
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
            var round = WA18;
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
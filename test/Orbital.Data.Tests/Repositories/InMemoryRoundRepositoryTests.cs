using Orbital.Data.Repositories;
using Orbital.Models.Domain;
using Xunit;

namespace Orbital.Data.Tests.Repositories
{
    public class InMemoryRoundRepositoryTests
    {
        [Fact]
        public void TestGetAll()
        {
            var rounds = new[]
            {
                new Round(1, null, "Category1", "Name1", true, new RoundTarget[0]),
                new Round(2, null, "Category2", "Name2", false, new RoundTarget[0])
            };

            var repo = InMemoryRoundRepository.New(rounds);

            var result = repo.GetAll();
            Assert.Equal(rounds, result);
        }

        [Fact]
        public void TestGetAllVariantsById()
        {
            var clubs = new[]
            {
                new Round(1, 3, "Category1", "Name1", true, new RoundTarget[0]),
                new Round(2, null, "Category2", "Name2", false, new RoundTarget[0])
            };

            var repo = InMemoryRoundRepository.New(clubs);

            var result1 = repo.GetAllVariantsById(1);
            Assert.Empty(result1);

            var result2 = repo.GetAllVariantsById(3);
            Assert.Equal(new[] { clubs[0] }, result2);
        }

        [Fact]
        public void TestGetById()
        {
            var round = new Round(1, null, "Category", "Name", true, new RoundTarget[0]);
            var repo = InMemoryRoundRepository.New(round);

            var result = repo.GetById(1);
            Assert.Equal(round, result);
        }
        [Fact]
        public void TestGetById_Missing()
        {
            var round = new Round(1, null, "Category", "Name", true, new RoundTarget[0]);
            var repo = InMemoryRoundRepository.New(round);

            var result = repo.GetById(2);
            Assert.Null(result);
        }

        [Fact]
        public void TestCreate()
        {
            var round = new Round(1, null, "Category", "Name", true, new RoundTarget[0]);
            var repo = InMemoryRoundRepository.New();

            var createResult = repo.Create(round);
            Assert.Equal(round, createResult);

            var result = repo.GetById(1);
            Assert.Equal(round, result);
        }

        [Fact]
        public void TestUpdate()
        {
            var round = new Round(1, null, "Category", "Name", true, new RoundTarget[0]);
            var repo = InMemoryRoundRepository.New(round);

            var newRound = new Round(1, null, "Category", "Name-Updated", true, new[] { new RoundTarget(2, ScoringType.Metric, new Length(3, LengthUnit.Feet), new Length(4, LengthUnit.Meters), 5) });

            var updateResult = repo.Update(newRound);
            Assert.Equal(newRound, updateResult);

            var result = repo.GetById(1);
            Assert.Equal(newRound, result);
        }

        [Fact]
        public void TestRemove()
        {
            var round = new Round(1, null, "Category", "Name", true, new RoundTarget[0]);
            var repo = InMemoryRoundRepository.New(round);

            var deleteResult = repo.Delete(round);
            Assert.True(deleteResult);

            var result = repo.GetAll();
            Assert.Empty(result);
        }
    }
}


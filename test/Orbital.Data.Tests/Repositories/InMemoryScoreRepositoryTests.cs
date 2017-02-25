using System;
using Orbital.Data.Repositories;
using Orbital.Models.Domain;
using Xunit;

namespace Orbital.Data.Tests.Repositories
{
    public class InMemoryScoreRepositoryTests
    {
        [Fact]
        public void TestGetAllByPersonId()
        {
            var scores = new[]
            {
                new Score(1, 2, 3, Bowstyle.Recurve, null, 4, 5, 6, 7, DateTime.Now, DateTime.Now, new ScoreTarget[0]),
                new Score(2, 3, 3, Bowstyle.Recurve, null, 4, 5, 6, 7, DateTime.Now, DateTime.Now, new ScoreTarget[0]),
            };

            var repo = InMemoryScoreRepository.New(scores);

            var result1 = repo.GetAllByPersonId(1);
            Assert.Empty(result1);

            var result2 = repo.GetAllByPersonId(2);
            Assert.Equal(new[] { scores[0] }, result2);
        }

        [Fact]
        public void TestGetAllByPersonIdEmpty()
        {
            var repo = InMemoryScoreRepository.New();

            var result = repo.GetAllByPersonId(1);
            Assert.Empty(result);
        }

        [Fact]
        public void TestGetAllByRoundId()
        {
            var scores = new[]
            {
                new Score(1, 2, 3, Bowstyle.Recurve, null, 4, 5, 6, 7, DateTime.Now, DateTime.Now, new ScoreTarget[0]),
                new Score(2, 2, 3, Bowstyle.Recurve, null, 5, 5, 6, 7, DateTime.Now, DateTime.Now, new ScoreTarget[0]),
            };

            var repo = InMemoryScoreRepository.New(scores);

            var result1 = repo.GetAllByRoundId(3);
            Assert.Empty(result1);

            var result2 = repo.GetAllByRoundId(4);
            Assert.Equal(new[] { scores[0] }, result2);
        }

        [Fact]
        public void TestGetAllByRoundIdEmpty()
        {
            var repo = InMemoryScoreRepository.New();

            var result = repo.GetAllByRoundId(1);
            Assert.Empty(result);
        }

        [Fact]
        public void TestGetAllByCompetitionId()
        {
            var scores = new[]
            {
                new Score(1, 2, 3, Bowstyle.Recurve, 8, 4, 5, 6, 7, DateTime.Now, DateTime.Now, new ScoreTarget[0]),
                new Score(2, 3, 3, Bowstyle.Recurve, 9, 4, 5, 6, 7, DateTime.Now, DateTime.Now, new ScoreTarget[0]),
            };

            var repo = InMemoryScoreRepository.New(scores);

            var result1 = repo.GetAllByCompetitionId(7);
            Assert.Empty(result1);

            var result2 = repo.GetAllByCompetitionId(8);
            Assert.Equal(new[] { scores[0] }, result2);
        }

        [Fact]
        public void TestGetAllByCompetitionIdNull()
        {
            var scores = new[]
            {
                new Score(1, 2, 3, Bowstyle.Recurve, null, 4, 5, 6, 7, DateTime.Now, DateTime.Now, new ScoreTarget[0]),
            };

            var repo = InMemoryScoreRepository.New(scores);

            var result1 = repo.GetAllByCompetitionId(1);
            Assert.Empty(result1);
        }

        [Fact]
        public void TestGetAllByCompetitionIdEmpty()
        {
            var repo = InMemoryScoreRepository.New();

            var result = repo.GetAllByCompetitionId(1);
            Assert.Empty(result);
        }

    }
}

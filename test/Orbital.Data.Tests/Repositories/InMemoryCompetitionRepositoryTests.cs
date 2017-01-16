using System;
using Orbital.Data.Repositories;
using Orbital.Models.Domain;
using Xunit;

namespace Orbital.Data.Tests.Repositories
{
    public class InMemoryCompetitionRepositoryTests
    {
        [Fact]
        public void TestGetAll()
        {
            var competitions = new[]
            {
                new Competition(1, "Competition1", new DateTime(2010, 1, 1), new DateTime(2010, 1, 1), new int[0] ),
                new Competition(2, "Competition2", new DateTime(2010, 2, 1), new DateTime(2010, 2, 2), new [] { 1 } )
            };

            var repo = InMemoryCompetitionRepository.New(competitions);

            var result = repo.GetAll();
            Assert.Equal(competitions, result);
        }

        [Fact]
        public void TestGetAllEmpty()
        {
            var repo = InMemoryCompetitionRepository.New();

            var result = repo.GetAll();
            Assert.Empty(result);
        }

        [Fact]
        public void TestGetById()
        {
            var competition = new Competition(1, "Competition1", new DateTime(2010, 1, 1), new DateTime(2010, 1, 1), new[] { 1 });
            var repo = InMemoryCompetitionRepository.New(competition);

            var result = repo.GetById(1);
            Assert.Equal(competition, result);
        }
        [Fact]
        public void TestGetById_Missing()
        {
            var competition = new Competition(1, "Competition1", new DateTime(2010, 1, 1), new DateTime(2010, 1, 1), new[] { 1 });
            var repo = InMemoryCompetitionRepository.New(competition);

            var result = repo.GetById(2);
            Assert.Null(result);
        }

        [Fact]
        public void TestCreate()
        {
            var competition = new Competition(1, "Competition1", new DateTime(2010, 1, 1), new DateTime(2010, 1, 1), new[] { 1 });
            var repo = InMemoryCompetitionRepository.New();

            var createResult = repo.Create(competition);
            Assert.Equal(competition, createResult);

            var result = repo.GetById(1);
            Assert.Equal(competition, result);
        }

        [Fact]
        public void TestUpdate()
        {
            var competition = new Competition(1, "Competition1", new DateTime(2010, 1, 1), new DateTime(2010, 1, 1), new[] { 1 });
            var repo = InMemoryCompetitionRepository.New(competition);

            var newCompetition = new Competition(1, "Competition1", new DateTime(2010, 1, 1), new DateTime(2010, 1, 1), new[] { 1 });

            var updateResult = repo.Update(newCompetition);
            Assert.Equal(newCompetition, updateResult);

            var result = repo.GetById(1);
            Assert.Equal(newCompetition, result);
        }

        [Fact]
        public void TestRemove()
        {
            var competition = new Competition(1, "Competition1", new DateTime(2010, 1, 1), new DateTime(2010, 1, 1), new[] { 1 });
            var repo = InMemoryCompetitionRepository.New(competition);

            var deleteResult = repo.Delete(competition);
            Assert.True(deleteResult);

            var result = repo.GetAll();
            Assert.Empty(result);
        }
    }
}

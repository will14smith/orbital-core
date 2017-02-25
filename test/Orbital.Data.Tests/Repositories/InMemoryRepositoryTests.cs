using System.Collections.Generic;
using System.Linq;
using Orbital.Data.Repositories;
using Orbital.Models.Domain;
using Xunit;

namespace Orbital.Data.Tests.Repositories
{
    public class InMemoryRepositoryTests
    {
        private class Repository : InMemoryRepository<Club>
        {
            public Repository(IEnumerable<Club> clubs) : base(clubs.ToList()) { }

            protected override int GetId(Club item) { return item.Id; }
        }

        [Fact]
        public void TestGetAll()
        {
            var clubs = new[]
            {
                new Club(1, "Club1"),
                new Club(2, "Club2")
            };

            var repo = new Repository(clubs);

            var result = repo.GetAll();
            Assert.Equal(clubs, result);
        }

        [Fact]
        public void TestGetAllEmpty()
        {
            var repo = new Repository(Enumerable.Empty<Club>());

            var result = repo.GetAll();
            Assert.Empty(result);
        }

        [Fact]
        public void TestGetById()
        {
            var club = new Club(1, "Club1");
            var repo = new Repository(new[] { club });

            var result = repo.GetById(1);
            Assert.Equal(club, result);
        }
        [Fact]
        public void TestGetById_Missing()
        {
            var club = new Club(1, "Club1");
            var repo = new Repository(new[] { club });

            var result = repo.GetById(2);
            Assert.Null(result);
        }

        [Fact]
        public void TestCreate()
        {
            var club = new Club(1, "Club1");
            var repo = new Repository(Enumerable.Empty<Club>());

            var createResult = repo.Create(club);
            Assert.Equal(club, createResult);

            var result = repo.GetById(1);
            Assert.Equal(club, result);
        }

        [Fact]
        public void TestUpdate()
        {
            var club = new Club(1, "Club1");
            var repo = new Repository(new[] { club });

            var newClub = new Club(1, "Club1-Updated");

            var updateResult = repo.Update(newClub);
            Assert.Equal(newClub, updateResult);

            var result = repo.GetById(1);
            Assert.Equal(newClub, result);
        }

        [Fact]
        public void TestRemove()
        {
            var club = new Club(1, "Club1");
            var repo = new Repository(new[] { club });

            var deleteResult = repo.Delete(club);
            Assert.True(deleteResult);

            var result = repo.GetAll();
            Assert.Empty(result);
        }
    }
}

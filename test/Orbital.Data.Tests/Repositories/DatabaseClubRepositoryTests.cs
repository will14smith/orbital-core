using Orbital.Data.Repositories;
using Orbital.Data.Tests.Helpers;
using Orbital.Models.Domain;
using System.Linq;
using Xunit;

namespace Orbital.Data.Tests.Repositories
{
    public class DatabaseClubRepositoryTests : DatabaseRepositoryTestBase
    {
        public DatabaseClubRepositoryTests(DatabaseFixture databaseFixture) : base(databaseFixture)
        {
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetNonExistant()
        {
            var repository = new DatabaseClubRepository(GetConnectionFactory());

            var result = repository.GetById(1);
            Assert.Null(result);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterCreate()
        {
            var repository = new DatabaseClubRepository(GetConnectionFactory());
            var club = new Club(0, "Club1");

            var insertResult = repository.Create(club);
            Assert.Equal(club.Name, insertResult.Name);

            var getResult = repository.GetById(insertResult.Id);

            Assert.Equal(insertResult, getResult);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllEmpty()
        {
            var repository = new DatabaseClubRepository(GetConnectionFactory());

            var getAllResult = repository.GetAll();
            Assert.Equal(0, getAllResult.Count);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllAfterInsert()
        {
            var repository = new DatabaseClubRepository(GetConnectionFactory());
            var club = new Club(0, "Club1");
            var insertResult = repository.Create(club);

            var getAllResult = repository.GetAll();
            Assert.Equal(1, getAllResult.Count);
            Assert.Equal(club.Name, getAllResult.First().Name);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterUpdate()
        {
            var repository = new DatabaseClubRepository(GetConnectionFactory());
            var club = new Club(0, "Club1");
            var insertResult = repository.Create(club);

            var updatedClub = new Club(insertResult.Id, "Club1-updated");
            var updateResult = repository.Update(updatedClub);
            Assert.Equal(updatedClub.Name, updateResult.Name);

            var getResult = repository.GetById(updateResult.Id);
            Assert.Equal(updateResult, getResult);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterRemove()
        {
            var repository = new DatabaseClubRepository(GetConnectionFactory());
            var club = new Club(0, "Club1");
            var insertResult = repository.Create(club);

            var removeResult = repository.Delete(insertResult);
            Assert.True(removeResult);

            var result = repository.GetById(insertResult.Id);
            Assert.Null(result);
        }
    }
}

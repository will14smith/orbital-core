using Orbital.Data.Repositories;
using Orbital.Data.Tests.Helpers;
using Orbital.Models.Domain;
using System.Linq;
using Xunit;

namespace Orbital.Data.Tests.Repositories
{
    public class DatabaseBadgeRepositoryTests : DatabaseRepositoryTestBase
    {
        public DatabaseBadgeRepositoryTests(DatabaseFixture databaseFixture) : base(databaseFixture)
        {
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetNonExistant()
        {
            var repository = new DatabaseBadgeRepository(ConnectionFactory);

            var result = repository.GetById(1);
            Assert.Null(result);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterCreate()
        {
            var repository = new DatabaseBadgeRepository(ConnectionFactory);
            var badge = new Badge(0, "Badge1", "Description", "Algo", "Cate", true, "Image");

            var insertResult = repository.Create(badge);
            Assert.Equal(badge.Name, insertResult.Name);

            var getResult = repository.GetById(insertResult.Id);

            Assert.Equal(insertResult, getResult);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllEmpty()
        {
            var repository = new DatabaseBadgeRepository(ConnectionFactory);

            var getAllResult = repository.GetAll();
            Assert.Equal(0, getAllResult.Count);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllAfterInsert()
        {
            var repository = new DatabaseBadgeRepository(ConnectionFactory);
            var badge = new Badge(0, "Badge1", "Description", "Algo", "Cate", true, "Image");
            var insertResult = repository.Create(badge);

            var getAllResult = repository.GetAll();
            Assert.Equal(1, getAllResult.Count);
            Assert.Equal(badge.Name, getAllResult.First().Name);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterUpdate()
        {
            var repository = new DatabaseBadgeRepository(ConnectionFactory);
            var badge = new Badge(0, "Badge1", "Description", "Algo", "Cate", true, "Image");
            var insertResult = repository.Create(badge);

            var updatedBadge = new Badge(insertResult.Id, "Badge1", "Description", "Algo", "Cate", true, "Image");
            var updateResult = repository.Update(updatedBadge);
            Assert.Equal(updatedBadge.Name, updateResult.Name);

            var getResult = repository.GetById(updateResult.Id);
            Assert.Equal(updateResult, getResult);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterRemove()
        {
            var repository = new DatabaseBadgeRepository(ConnectionFactory);
            var badge = new Badge(0, "Badge1", "Description", "Algo", "Cate", true, "Image");
            var insertResult = repository.Create(badge);

            var removeResult = repository.Delete(insertResult);
            Assert.True(removeResult);

            var result = repository.GetById(insertResult.Id);
            Assert.Null(result);
        }
    }
}

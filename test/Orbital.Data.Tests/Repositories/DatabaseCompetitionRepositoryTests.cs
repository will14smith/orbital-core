using Orbital.Data.Repositories;
using Orbital.Data.Tests.Helpers;
using Orbital.Models.Domain;
using System;
using System.Linq;
using Xunit;

namespace Orbital.Data.Tests.Repositories
{
    public class DatabaseCompetitionRepositoryTests : DatabaseRepositoryTestBase
    {
        public DatabaseCompetitionRepositoryTests(DatabaseFixture databaseFixture) : base(databaseFixture)
        {
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetNonExistant()
        {
            var repository = new DatabaseCompetitionRepository(ConnectionFactory);

            var result = repository.GetById(1);
            Assert.Null(result);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterCreate()
        {
            var repository = new DatabaseCompetitionRepository(ConnectionFactory);

            var round = DataFixtures.GetRound(ConnectionFactory);
            var competition = new Competition(0, "Name", new DateTime(2010, 1, 1), new DateTime(2011, 2, 3), new[] { round });

            var insertResult = repository.Create(competition);
            var getResult = repository.GetById(insertResult.Id);
            Assert.Equal(insertResult, getResult);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetWithNoTargets()
        {
            var repository = new DatabaseCompetitionRepository(ConnectionFactory);
            var competition = new Competition(0, "Name", new DateTime(2010, 1, 1), new DateTime(2011, 2, 3), new int[0]);

            var insertResult = repository.Create(competition);
            var getResult = repository.GetById(insertResult.Id);
            Assert.Equal(insertResult, getResult);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllEmpty()
        {
            var repository = new DatabaseCompetitionRepository(ConnectionFactory);

            var getAllResult = repository.GetAll();
            Assert.Equal(0, getAllResult.Count);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllAfterInsert()
        {
            var repository = new DatabaseCompetitionRepository(ConnectionFactory);

            var round1 = DataFixtures.GetRound(ConnectionFactory);
            var round2 = DataFixtures.GetRound(ConnectionFactory);

            var competition1 = new Competition(0, "Name1", new DateTime(2010, 1, 1), new DateTime(2011, 2, 3), new[] { round1 });
            var competition2 = new Competition(0, "Name2", new DateTime(2010, 1, 1), new DateTime(2011, 2, 3), new[] { round1, round2 });

            var insert1 = repository.Create(competition1);
            var insert2 = repository.Create(competition2);

            var getAllResult = repository.GetAll();
            Assert.Equal(2, getAllResult.Count);
            Assert.Equal(insert1, getAllResult.First(x => x.Name == competition1.Name));
            Assert.Equal(insert2, getAllResult.First(x => x.Name == competition2.Name));
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterUpdate()
        {
            var repository = new DatabaseCompetitionRepository(ConnectionFactory);
            var round1 = DataFixtures.GetRound(ConnectionFactory);
            var round2 = DataFixtures.GetRound(ConnectionFactory);

            var competition1 = new Competition(0, "Name", new DateTime(2010, 1, 1), new DateTime(2011, 2, 3), new[] { round1 });
            var insertResult = repository.Create(competition1);

            var updatedCompetition = new Competition(insertResult.Id, "Name1", new DateTime(2010, 1, 2), new DateTime(2011, 2, 3), new[] { round2 });
            var updateResult = repository.Update(updatedCompetition);

            Assert.Equal(updatedCompetition, updateResult);

            var getResult = repository.GetById(updateResult.Id);
            Assert.Equal(updateResult, getResult);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterRemove()
        {
            var repository = new DatabaseCompetitionRepository(ConnectionFactory);
            var round = DataFixtures.GetRound(ConnectionFactory);
            var competition = new Competition(0, "Name", new DateTime(2010, 1, 1), new DateTime(2011, 2, 3), new[] { round });
            var insertResult = repository.Create(competition);

            var removeResult = repository.Delete(insertResult);
            Assert.True(removeResult);

            var result = repository.GetById(insertResult.Id);
            Assert.Null(result);
        }
    }
}

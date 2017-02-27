using Orbital.Data.Repositories;
using Orbital.Data.Tests.Helpers;
using Orbital.Models.Domain;
using System.Linq;
using Xunit;

namespace Orbital.Data.Tests.Repositories
{
    public class DatabaseRoundRepositoryTests : DatabaseRepositoryTestBase
    {
        public DatabaseRoundRepositoryTests(DatabaseFixture databaseFixture) : base(databaseFixture)
        {
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetNonExistant()
        {
            var repository = new DatabaseRoundRepository(ConnectionFactory);

            var result = repository.GetById(1);
            Assert.Null(result);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterCreate()
        {
            var repository = new DatabaseRoundRepository(ConnectionFactory);
            var round = new Round(0, null, "Category", "Name", true, new[] { new RoundTarget(0, ScoringType.Imperial, new Length(2, LengthUnit.Centimeters), new Length(1.5m, LengthUnit.Yards), 36) });

            var insertResult = repository.Create(round);
            Assert.Equal(round, insertResult, new Round.EqualWithoutId());

            var getResult = repository.GetById(insertResult.Id);
            Assert.Equal(insertResult, getResult, new Round.EqualWithoutId());
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetWithNoTargets()
        {
            var repository = new DatabaseRoundRepository(ConnectionFactory);
            var round = new Round(0, null, "Category", "Name", true, new RoundTarget[0]);

            var insertResult = repository.Create(round);
            Assert.Equal(round, insertResult, new Round.EqualWithoutId());

            var getResult = repository.GetById(insertResult.Id);
            Assert.Equal(insertResult, getResult, new Round.EqualWithoutId());
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllEmpty()
        {
            var repository = new DatabaseRoundRepository(ConnectionFactory);

            var getAllResult = repository.GetAll();
            Assert.Equal(0, getAllResult.Count);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllAfterInsert()
        {
            var repository = new DatabaseRoundRepository(ConnectionFactory);
            var round1 = new Round(0, null, "Category1", "Name1", true, new[] { new RoundTarget(0, ScoringType.Imperial, new Length(2, LengthUnit.Centimeters), new Length(1.5m, LengthUnit.Yards), 36) });
            var round2 = new Round(0, null, "Category2", "Name2", true, new[] { new RoundTarget(0, ScoringType.FiveZone, new Length(4, LengthUnit.Centimeters), new Length(5.5m, LengthUnit.Yards), 50) });

            repository.Create(round1);
            repository.Create(round2);

            var getAllResult = repository.GetAll();
            Assert.Equal(2, getAllResult.Count);
            Assert.Equal(round1, getAllResult.First(x => x.Name == round1.Name), new Round.EqualWithoutId());
            Assert.Equal(round2, getAllResult.First(x => x.Name == round2.Name), new Round.EqualWithoutId());
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllVariantsByIdEmpty()
        {
            var repository = new DatabaseRoundRepository(ConnectionFactory);

            var result = repository.GetAllVariantsById(1);
            Assert.Empty(result);
        }
        [Fact, Trait("Type", "Integration")]
        public void TestGetAllVariantsByIdAfterInsert()
        {
            var repository = new DatabaseRoundRepository(ConnectionFactory);
            var round1 = new Round(0, null, "Category1", "Name1", true, new[] { new RoundTarget(0, ScoringType.Imperial, new Length(2, LengthUnit.Centimeters), new Length(1.5m, LengthUnit.Yards), 36) });
            var insertResult1 = repository.Create(round1);

            var round2 = new Round(0, insertResult1.Id, "Category2", "Name2", true, new[] { new RoundTarget(0, ScoringType.FiveZone, new Length(4, LengthUnit.Centimeters), new Length(5.5m, LengthUnit.Yards), 50) });
            var insertResult2 = repository.Create(round2);

            var result = repository.GetAllVariantsById(insertResult1.Id);
            Assert.Equal(new[] { insertResult2.Id }, result.Select(x => x.Id));
        }
        [Fact, Trait("Type", "Integration")]
        public void TestGetAllVariantsByIdAfterInsertEmpty()
        {
            var repository = new DatabaseRoundRepository(ConnectionFactory);
            var round1 = new Round(0, null, "Category1", "Name1", true, new[] { new RoundTarget(0, ScoringType.Imperial, new Length(2, LengthUnit.Centimeters), new Length(1.5m, LengthUnit.Yards), 36) });
            var insertResult1 = repository.Create(round1);

            var round2 = new Round(0, insertResult1.Id, "Category2", "Name2", true, new[] { new RoundTarget(0, ScoringType.FiveZone, new Length(4, LengthUnit.Centimeters), new Length(5.5m, LengthUnit.Yards), 50) });
            var insertResult2 = repository.Create(round2);

            var result = repository.GetAllVariantsById(insertResult2.Id);
            Assert.Empty(result);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterUpdate()
        {
            var repository = new DatabaseRoundRepository(ConnectionFactory);
            var round = new Round(0, null, "Category1", "Name1", true, new[] { new RoundTarget(0, ScoringType.Imperial, new Length(2, LengthUnit.Centimeters), new Length(1.5m, LengthUnit.Yards), 36) });
            var insertResult = repository.Create(round);

            var updatedRound = new Round(insertResult.Id, null, "Category2", "Name2", true, new[] { new RoundTarget(0, ScoringType.FiveZone, new Length(4, LengthUnit.Centimeters), new Length(5.5m, LengthUnit.Yards), 50) });
            var updateResult = repository.Update(updatedRound);

            Assert.Equal(updatedRound, updateResult, new Round.EqualWithoutId());

            var getResult = repository.GetById(updateResult.Id);
            Assert.Equal(updateResult, getResult, new Round.EqualWithoutId());
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterRemove()
        {
            var repository = new DatabaseRoundRepository(ConnectionFactory);
            var round = new Round(0, null, "Category", "Name", true, new RoundTarget[0]);
            var insertResult = repository.Create(round);

            var removeResult = repository.Delete(insertResult);
            Assert.True(removeResult);

            var result = repository.GetById(insertResult.Id);
            Assert.Null(result);
        }
    }
}

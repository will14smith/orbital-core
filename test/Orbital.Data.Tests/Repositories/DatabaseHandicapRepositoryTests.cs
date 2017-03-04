using System;
using System.Collections.Generic;
using Orbital.Data.Repositories;
using Orbital.Data.Tests.Helpers;
using Orbital.Models.Domain;
using System.Linq;
using Xunit;

namespace Orbital.Data.Tests.Repositories
{
    public class DatabaseHandicapRepositoryTests : DatabaseRepositoryTestBase
    {
        public DatabaseHandicapRepositoryTests(DatabaseFixture databaseFixture) : base(databaseFixture)
        {
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetNonExistant()
        {
            var repository = new DatabaseHandicapRepository(ConnectionFactory);

            var result = repository.GetById(1);
            Assert.Null(result);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterCreate()
        {
            var repository = new DatabaseHandicapRepository(ConnectionFactory);
            var handicap = new Handicap(0, DataFixtures.GetPerson(ConnectionFactory), DataFixtures.GetScore(ConnectionFactory), HandicapType.Initial, new DateTime(2017, 1, 1), 10, new HandicapIdentifier(true, Bowstyle.Recurve));

            var insertResult = repository.Create(handicap);

            Assert.Equal(handicap.PersonId, insertResult.PersonId);
            Assert.Equal(handicap.ScoreId, insertResult.ScoreId);

            Assert.Equal(handicap.Type, insertResult.Type);
            Assert.Equal(handicap.Date, insertResult.Date);
            Assert.Equal(handicap.Value, insertResult.Value);

            Assert.Equal(handicap.Identifier.Indoor, insertResult.Identifier.Indoor);
            Assert.Equal(handicap.Identifier.Bowstyle, insertResult.Identifier.Bowstyle);
            
            var getResult = repository.GetById(insertResult.Id);
            Assert.Equal(insertResult, getResult);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllEmpty()
        {
            var repository = new DatabaseHandicapRepository(ConnectionFactory);

            var getAllResult = repository.GetAll();
            Assert.Equal(0, getAllResult.Count);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllAfterInsert()
        {
            var repository = new DatabaseHandicapRepository(ConnectionFactory);

            var handicap = repository.Create(new Handicap(0, DataFixtures.GetPerson(ConnectionFactory), DataFixtures.GetScore(ConnectionFactory), HandicapType.Initial, new DateTime(2017, 1, 1), 10, new HandicapIdentifier(true, Bowstyle.Recurve)));

            var getAllResult = repository.GetAll();
            Assert.Equal(1, getAllResult.Count);
            Assert.Equal(handicap, getAllResult.First());
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllByPersonId_CorrectPerson()
        {
            var repository = new DatabaseHandicapRepository(ConnectionFactory);

            var person1 = DataFixtures.GetPerson(ConnectionFactory);
            var handicap1 = repository.Create(new Handicap(0, person1, DataFixtures.GetScore(ConnectionFactory), HandicapType.Initial, new DateTime(2017, 1, 1), 10, new HandicapIdentifier(true, Bowstyle.Recurve)));
            var person2 = DataFixtures.GetPerson(ConnectionFactory);
            repository.Create(new Handicap(0, person2, DataFixtures.GetScore(ConnectionFactory), HandicapType.Initial, new DateTime(2017, 1, 1), 10, new HandicapIdentifier(true, Bowstyle.Recurve)));

            var result = repository.GetAllByPersonId(person1);
            Assert.Equal(1, result.Count);
            Assert.Equal(1, result.First().Count());
            Assert.Equal(handicap1, result.First().First());
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllByPersonId_CorrectGrouping()
        {
            var repository = new DatabaseHandicapRepository(ConnectionFactory);

            var person = DataFixtures.GetPerson(ConnectionFactory);
            repository.Create(new Handicap(0, person, DataFixtures.GetScore(ConnectionFactory), HandicapType.Initial, new DateTime(2017, 1, 1), 10, new HandicapIdentifier(true, Bowstyle.Recurve)));
            repository.Create(new Handicap(0, person, DataFixtures.GetScore(ConnectionFactory), HandicapType.Initial, new DateTime(2017, 1, 2), 20, new HandicapIdentifier(true, Bowstyle.Barebow)));

            var result = repository.GetAllByPersonId(person);
            Assert.Equal(2, result.Count);
            Assert.Contains(new HandicapIdentifier(true, Bowstyle.Recurve), result.Select(x => x.Key));
            Assert.Contains(new HandicapIdentifier(true, Bowstyle.Barebow), result.Select(x => x.Key));
        }

        public void TestGetAllLatestByPersonId()
        {
            var repository = new DatabaseHandicapRepository(ConnectionFactory);

            var person1 = DataFixtures.GetPerson(ConnectionFactory);
            // this is older than hc2
            repository.Create(new Handicap(0, person1, DataFixtures.GetScore(ConnectionFactory), HandicapType.Initial, new DateTime(2017, 1, 1), 10, new HandicapIdentifier(true, Bowstyle.Recurve)));
            // this is latest recurve
            var handicap2 = repository.Create(new Handicap(0, person1, DataFixtures.GetScore(ConnectionFactory), HandicapType.Initial, new DateTime(2017, 1, 2), 5, new HandicapIdentifier(true, Bowstyle.Recurve)));
            // this is latest barebow
            var handicap3 = repository.Create(new Handicap(0, person1, DataFixtures.GetScore(ConnectionFactory), HandicapType.Initial, new DateTime(2017, 1, 3), 20, new HandicapIdentifier(true, Bowstyle.Barebow)));
            var person2 = DataFixtures.GetPerson(ConnectionFactory);
            // this is a later barebow for a different person
            repository.Create(new Handicap(0, person2, DataFixtures.GetScore(ConnectionFactory), HandicapType.Initial, new DateTime(2017, 1, 4), 15, new HandicapIdentifier(true, Bowstyle.Barebow)));

            var result = repository.GetLatestByPersonId(person1);
            Assert.Equal(2, result.Count);
            Assert.Equal(new Dictionary<HandicapIdentifier, Handicap>
            {
                { handicap2.Identifier, handicap2 },
                { handicap3.Identifier, handicap3 },
            }, result);
        }

        public void TestGetLatestByPersonId()
        {
            var repository = new DatabaseHandicapRepository(ConnectionFactory);

            var person1 = DataFixtures.GetPerson(ConnectionFactory);
            // this is older than hc2
            repository.Create(new Handicap(0, person1, DataFixtures.GetScore(ConnectionFactory), HandicapType.Initial, new DateTime(2017, 1, 1), 10, new HandicapIdentifier(true, Bowstyle.Recurve)));
            // this is latest recurve
            var handicap2 = repository.Create(new Handicap(0, person1, DataFixtures.GetScore(ConnectionFactory), HandicapType.Initial, new DateTime(2017, 1, 2), 5, new HandicapIdentifier(true, Bowstyle.Recurve)));
            // this is later but a different type
            repository.Create(new Handicap(0, person1, DataFixtures.GetScore(ConnectionFactory), HandicapType.Initial, new DateTime(2017, 1, 3), 20, new HandicapIdentifier(true, Bowstyle.Barebow)));
            var person2 = DataFixtures.GetPerson(ConnectionFactory);
            // this is a later recurve for a different person
            repository.Create(new Handicap(0, person2, DataFixtures.GetScore(ConnectionFactory), HandicapType.Initial, new DateTime(2017, 1, 4), 15, new HandicapIdentifier(true, Bowstyle.Recurve)));

            var result = repository.GetLatestByPersonId(handicap2.Identifier, person1);
            Assert.Equal(handicap2, result);
        }

        public void TestGetByScoreId()
        {
            var repository = new DatabaseHandicapRepository(ConnectionFactory);

            var score1 = DataFixtures.GetScore(ConnectionFactory);
            var handicap1 = repository.Create(new Handicap(0, DataFixtures.GetPerson(ConnectionFactory), score1, HandicapType.Initial, new DateTime(2017, 1, 1), 10, new HandicapIdentifier(true, Bowstyle.Recurve)));
            var score2 = DataFixtures.GetScore(ConnectionFactory);
            repository.Create(new Handicap(0, DataFixtures.GetPerson(ConnectionFactory), score2, HandicapType.Initial, new DateTime(2017, 1, 2), 20, new HandicapIdentifier(true, Bowstyle.Barebow)));

            var result = repository.GetByScoreId(score1);
            Assert.Equal(handicap1, result);
        }
        public void TestGetByScoreId_Missing()
        {
            var repository = new DatabaseHandicapRepository(ConnectionFactory);

            var result = repository.GetByScoreId(1);
            Assert.Null(result);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterUpdate()
        {
            var repository = new DatabaseHandicapRepository(ConnectionFactory);
            var handicap = new Handicap(0, DataFixtures.GetPerson(ConnectionFactory), DataFixtures.GetScore(ConnectionFactory), HandicapType.Initial, new DateTime(2017, 1, 1), 10, new HandicapIdentifier(true, Bowstyle.Recurve));
            var insertResult = repository.Create(handicap);

            var updatedHandicap = new Handicap(insertResult.Id, DataFixtures.GetPerson(ConnectionFactory), DataFixtures.GetScore(ConnectionFactory), HandicapType.Update, new DateTime(2017, 2, 2), 20, new HandicapIdentifier(false, Bowstyle.Barebow));
            var updateResult = repository.Update(updatedHandicap);

            Assert.Equal(updatedHandicap, updateResult);

            var getResult = repository.GetById(updateResult.Id);
            Assert.Equal(updateResult, getResult);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterRemove()
        {
            var repository = new DatabaseHandicapRepository(ConnectionFactory);
            var handicap = new Handicap(0, DataFixtures.GetPerson(ConnectionFactory), DataFixtures.GetScore(ConnectionFactory), HandicapType.Initial, new DateTime(2017, 1, 1), 10, new HandicapIdentifier(true, Bowstyle.Recurve));
            var insertResult = repository.Create(handicap);

            var removeResult = repository.Delete(insertResult);
            Assert.True(removeResult);

            var result = repository.GetById(insertResult.Id);
            Assert.Null(result);
        }
    }
}

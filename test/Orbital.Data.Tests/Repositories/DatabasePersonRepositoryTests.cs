using Orbital.Data.Repositories;
using Orbital.Data.Tests.Helpers;
using Orbital.Models.Domain;
using System;
using System.Linq;
using Xunit;

namespace Orbital.Data.Tests.Repositories
{
    public class DatabasePersonRepositoryTests : DatabaseRepositoryTestBase
    {
        public DatabasePersonRepositoryTests(DatabaseFixture databaseFixture) : base(databaseFixture)
        {
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetNonExistant()
        {
            var repository = new DatabasePersonRepository(ConnectionFactory);

            var result = repository.GetById(1);
            Assert.Null(result);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterCreate()
        {
            var repository = new DatabasePersonRepository(ConnectionFactory);
            var person = new Person(0, DataFixtures.GetClub(ConnectionFactory), "Person1", Gender.Female, Bowstyle.Recurve, "AGB", new DateTime(2010, 10, 2), new DateTime(2011, 2, 17));

            var insertResult = repository.Create(person);

            Assert.Equal(person.ClubId, insertResult.ClubId);
            Assert.Equal(person.Name, insertResult.Name);
            Assert.Equal(person.Gender, insertResult.Gender);
            Assert.Equal(person.Bowstyle, insertResult.Bowstyle);
            Assert.Equal(person.ArcheryGBNumber, insertResult.ArcheryGBNumber);
            Assert.Equal(person.DateOfBirth, insertResult.DateOfBirth);
            Assert.Equal(person.DateStartedArchery, insertResult.DateStartedArchery);

            var getResult = repository.GetById(insertResult.Id);

            Assert.Equal(insertResult, getResult);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllEmpty()
        {
            var repository = new DatabasePersonRepository(ConnectionFactory);

            var getAllResult = repository.GetAll();
            Assert.Equal(0, getAllResult.Count);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllAfterInsert()
        {
            var repository = new DatabasePersonRepository(ConnectionFactory);
            var person = new Person(0, DataFixtures.GetClub(ConnectionFactory), "Person1", Gender.Female);
            var insertResult = repository.Create(person);

            var getAllResult = repository.GetAll();
            Assert.Equal(1, getAllResult.Count);
            Assert.Equal(person.Name, getAllResult.First().Name);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllByClubEmpty()
        {
            var repository = new DatabasePersonRepository(ConnectionFactory);
            var club = DataFixtures.GetClub(ConnectionFactory);

            var getAllResult = repository.GetAllByClubId(club);
            Assert.Equal(0, getAllResult.Count);
        }
        [Fact, Trait("Type", "Integration")]
        public void TestGetAllByClubAfterInsert()
        {
            var repository = new DatabasePersonRepository(ConnectionFactory);
            var club = DataFixtures.GetClub(ConnectionFactory);
            var person = new Person(0, club, "Person1", Gender.Female);
            var insertResult = repository.Create(person);

            var getAllResult = repository.GetAllByClubId(club);
            Assert.Equal(1, getAllResult.Count);
            Assert.Equal(person.Name, getAllResult.First().Name);
        }
        [Fact, Trait("Type", "Integration")]
        public void TestGetAllByClubAfterInsertEmpty()
        {
            var repository = new DatabasePersonRepository(ConnectionFactory);
            var club1 = DataFixtures.GetClub(ConnectionFactory);
            var club2 = DataFixtures.GetClub(ConnectionFactory);
            var person = new Person(0, club1, "Person1", Gender.Female);
            var insertResult = repository.Create(person);

            var getAllResult = repository.GetAllByClubId(club2);
            Assert.Equal(0, getAllResult.Count);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterUpdate()
        {
            var repository = new DatabasePersonRepository(ConnectionFactory);
            var club1 = DataFixtures.GetClub(ConnectionFactory);
            var club2 = DataFixtures.GetClub(ConnectionFactory);
            var person = new Person(0, club1, "Person1", Gender.Female, Bowstyle.Recurve, "AGB", new DateTime(2010, 10, 2), new DateTime(2011, 2, 17));
            var insertResult = repository.Create(person);

            var updatedPerson = new Person(insertResult.Id, club2, "Person1", Gender.Male);
            var updateResult = repository.Update(updatedPerson);

            Assert.Equal(updatedPerson.ClubId, updateResult.ClubId);
            Assert.Equal(updatedPerson.Name, updateResult.Name);
            Assert.Equal(updatedPerson.Gender, updateResult.Gender);
            Assert.Equal(updatedPerson.Bowstyle, updateResult.Bowstyle);
            Assert.Equal(updatedPerson.ArcheryGBNumber, updateResult.ArcheryGBNumber);
            Assert.Equal(updatedPerson.DateOfBirth, updateResult.DateOfBirth);
            Assert.Equal(updatedPerson.DateStartedArchery, updateResult.DateStartedArchery);

            var getResult = repository.GetById(updateResult.Id);
            Assert.Equal(updateResult, getResult);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterRemove()
        {
            var repository = new DatabasePersonRepository(ConnectionFactory);
            var club = DataFixtures.GetClub(ConnectionFactory);
            var person = new Person(0, club, "Person1", Gender.Female);
            var insertResult = repository.Create(person);

            var removeResult = repository.Delete(insertResult);
            Assert.True(removeResult);

            var result = repository.GetById(insertResult.Id);
            Assert.Null(result);
        }
    }
}

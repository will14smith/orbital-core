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
            var repository = new DatabasePersonRepository(GetConnectionFactory());

            var result = repository.GetById(1);
            Assert.Null(result);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterCreate()
        {
            var repository = new DatabasePersonRepository(GetConnectionFactory());
            var person = new Person(0, GetClub().Id, "Person1", Gender.Female, Bowstyle.Recurve, "AGB", new DateTime(2010, 10, 2), new DateTime(2011, 2, 17));

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
            var repository = new DatabasePersonRepository(GetConnectionFactory());

            var getAllResult = repository.GetAll();
            Assert.Equal(0, getAllResult.Count);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllAfterInsert()
        {
            var repository = new DatabasePersonRepository(GetConnectionFactory());
            var person = new Person(0, GetClub().Id, "Person1", Gender.Female);
            var insertResult = repository.Create(person);

            var getAllResult = repository.GetAll();
            Assert.Equal(1, getAllResult.Count);
            Assert.Equal(person.Name, getAllResult.First().Name);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllByClubEmpty()
        {
            var repository = new DatabasePersonRepository(GetConnectionFactory());
            var club = GetClub();

            var getAllResult = repository.GetAllByClubId(club.Id);
            Assert.Equal(0, getAllResult.Count);
        }
        [Fact, Trait("Type", "Integration")]
        public void TestGetAllByClubAfterInsert()
        {
            var repository = new DatabasePersonRepository(GetConnectionFactory());
            var club = GetClub();
            var person = new Person(0, club.Id, "Person1", Gender.Female);
            var insertResult = repository.Create(person);

            var getAllResult = repository.GetAllByClubId(club.Id);
            Assert.Equal(1, getAllResult.Count);
            Assert.Equal(person.Name, getAllResult.First().Name);
        }
        [Fact, Trait("Type", "Integration")]
        public void TestGetAllByClubAfterInsertEmpty()
        {
            var repository = new DatabasePersonRepository(GetConnectionFactory());
            var club1 = GetClub();
            var club2 = GetClub();
            var person = new Person(0, club1.Id, "Person1", Gender.Female);
            var insertResult = repository.Create(person);

            var getAllResult = repository.GetAllByClubId(club2.Id);
            Assert.Equal(0, getAllResult.Count);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterUpdate()
        {
            var repository = new DatabasePersonRepository(GetConnectionFactory());
            var club1 = GetClub();
            var club2 = GetClub();
            var person = new Person(0, club1.Id, "Person1", Gender.Female, Bowstyle.Recurve, "AGB", new DateTime(2010, 10, 2), new DateTime(2011, 2, 17));
            var insertResult = repository.Create(person);

            var updatedPerson = new Person(insertResult.Id, club2.Id, "Person1", Gender.Male);
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
            var repository = new DatabasePersonRepository(GetConnectionFactory());
            var club = GetClub();
            var person = new Person(0, club.Id, "Person1", Gender.Female);
            var insertResult = repository.Create(person);

            var removeResult = repository.Delete(insertResult);
            Assert.True(removeResult);

            var result = repository.GetById(insertResult.Id);
            Assert.Null(result);
        }

        private int clubCounter = 1;
        private Club GetClub()
        {
            var repository = new DatabaseClubRepository(GetConnectionFactory());
            var club = new Club(0, "Club" + (clubCounter++));
            return repository.Create(club);
        }
    }
}

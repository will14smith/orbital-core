using System;
using Orbital.Data.Repositories;
using Orbital.Data.Tests.Helpers;
using Orbital.Models.Domain;
using System.Linq;
using Xunit;
using Record = Orbital.Models.Domain.Record;

namespace Orbital.Data.Tests.Repositories
{
    public class DatabaseRecordRepositoryTests : DatabaseRepositoryTestBase
    {
        public DatabaseRecordRepositoryTests(DatabaseFixture databaseFixture) : base(databaseFixture)
        {
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetNonExistant()
        {
            var repository = new DatabaseRecordRepository(ConnectionFactory);

            var result = repository.GetById(1);
            Assert.Null(result);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterCreate()
        {
            var repository = new DatabaseRecordRepository(ConnectionFactory);
            var record = new Record(0, 1, new[]
            {
                new RecordClub(DataFixtures.GetClub(ConnectionFactory), new DateTime(2017, 1,1), new DateTime(2017, 2, 2))
            }, new[]
            {
                new RecordRound(DataFixtures.GetRound(ConnectionFactory), 1, Skill.Experienced, Bowstyle.Recurve, Gender.Male),
            });

            var insertResult = repository.Create(record);

            Assert.Equal(record.TeamSize, insertResult.TeamSize);

            Assert.Equal(record.Clubs.Single().ClubId, insertResult.Clubs.Single().ClubId);
            Assert.Equal(record.Clubs.Single().ActiveFrom, insertResult.Clubs.Single().ActiveFrom);
            Assert.Equal(record.Clubs.Single().ActiveTo, insertResult.Clubs.Single().ActiveTo);

            Assert.Equal(record.Rounds.Single().RoundId, insertResult.Rounds.Single().RoundId);
            Assert.Equal(record.Rounds.Single().Count, insertResult.Rounds.Single().Count);
            Assert.Equal(record.Rounds.Single().Skill, insertResult.Rounds.Single().Skill);
            Assert.Equal(record.Rounds.Single().Bowstyle, insertResult.Rounds.Single().Bowstyle);
            Assert.Equal(record.Rounds.Single().Gender, insertResult.Rounds.Single().Gender);

            var getResult = repository.GetById(insertResult.Id);
            Assert.Equal(insertResult, getResult);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllEmpty()
        {
            var repository = new DatabaseRecordRepository(ConnectionFactory);

            var getAllResult = repository.GetAll();
            Assert.Equal(0, getAllResult.Count);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllAfterInsert()
        {
            var repository = new DatabaseRecordRepository(ConnectionFactory);
            var record = repository.Create(new Record(0, 1, new[]
            {
                new RecordClub(DataFixtures.GetClub(ConnectionFactory), new DateTime(2017, 1,1), new DateTime(2017, 2, 2))
            }, new[]
            {
                new RecordRound(DataFixtures.GetRound(ConnectionFactory), 1, Skill.Experienced, Bowstyle.Recurve, Gender.Male),
            }));

            var result = repository.GetAll();
            Assert.Equal(1, result.Count);
            Assert.Equal(record, result.First());
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllByClubId()
        {
            var repository = new DatabaseRecordRepository(ConnectionFactory);

            var club1 = DataFixtures.GetClub(ConnectionFactory);
            var record1 = repository.Create(new Record(0, 1, new[] { new RecordClub(club1, new DateTime(2017, 1, 1), new DateTime(2017, 2, 2)) }, new[] { new RecordRound(DataFixtures.GetRound(ConnectionFactory), 1, Skill.Experienced, Bowstyle.Recurve, Gender.Male) }));
            var club2 = DataFixtures.GetClub(ConnectionFactory);
            repository.Create(new Record(0, 1, new[] { new RecordClub(club2, new DateTime(2017, 1, 1), new DateTime(2017, 2, 2)) }, new[] { new RecordRound(DataFixtures.GetRound(ConnectionFactory), 2, Skill.Experienced, Bowstyle.Recurve, Gender.Male) }));

            var result = repository.GetAllByClubId(club1);
            Assert.Equal(record1, result.Single());
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllByRoundId()
        {
            var repository = new DatabaseRecordRepository(ConnectionFactory);

            var round1 = DataFixtures.GetRound(ConnectionFactory);
            var record1 = repository.Create(new Record(0, 1, new[] { new RecordClub(DataFixtures.GetClub(ConnectionFactory), new DateTime(2017, 1, 1), new DateTime(2017, 2, 2)) }, new[] { new RecordRound(round1, 1, Skill.Experienced, Bowstyle.Recurve, Gender.Male) }));
            var round2 = DataFixtures.GetRound(ConnectionFactory);
            repository.Create(new Record(0, 1, new[] { new RecordClub(DataFixtures.GetClub(ConnectionFactory), new DateTime(2017, 1, 1), new DateTime(2017, 2, 2)) }, new[] { new RecordRound(round2, 2, Skill.Experienced, Bowstyle.Recurve, Gender.Male) }));

            var result = repository.GetAllByRoundId(round1);
            Assert.Equal(record1, result.Single());
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterUpdate()
        {
            var repository = new DatabaseRecordRepository(ConnectionFactory);
            var record = repository.Create(new Record(0, 1, new[]
            {
                new RecordClub(DataFixtures.GetClub(ConnectionFactory), new DateTime(2017, 1,1), new DateTime(2017, 2, 2))
            }, new[]
            {
                new RecordRound(DataFixtures.GetRound(ConnectionFactory), 1, Skill.Experienced, Bowstyle.Recurve, Gender.Male),
            }));
            var insertResult = repository.Create(record);

            var updatedRecord = repository.Create(new Record(insertResult.Id, 2, new[]
            {
                new RecordClub(DataFixtures.GetClub(ConnectionFactory), new DateTime(2017, 3,3), new DateTime(2017, 4, 4))
            }, new[]
            {
                new RecordRound(DataFixtures.GetRound(ConnectionFactory), 2, Skill.Novice, Bowstyle.Barebow, Gender.Female),
            }));
            var updateResult = repository.Update(updatedRecord);

            var getResult = repository.GetById(updateResult.Id);
            Assert.Equal(updateResult, getResult);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterRemove()
        {
            var repository = new DatabaseRecordRepository(ConnectionFactory);
            var record = repository.Create(new Record(0, 1, new[]
            {
                new RecordClub(DataFixtures.GetClub(ConnectionFactory), new DateTime(2017, 1,1), new DateTime(2017, 2, 2))
            }, new[]
            {
                new RecordRound(DataFixtures.GetRound(ConnectionFactory), 1, Skill.Experienced, Bowstyle.Recurve, Gender.Male),
            }));
            var insertResult = repository.Create(record);

            var removeResult = repository.Delete(insertResult);
            Assert.True(removeResult);

            var result = repository.GetById(insertResult.Id);
            Assert.Null(result);
        }
    }
}

using System;
using Orbital.Data.Repositories;
using Orbital.Data.Tests.Helpers;
using Orbital.Models.Domain;
using System.Linq;
using Xunit;

namespace Orbital.Data.Tests.Repositories
{
    public class DatabaseScoreRepositoryTests : DatabaseRepositoryTestBase
    {
        public DatabaseScoreRepositoryTests(DatabaseFixture databaseFixture) : base(databaseFixture)
        {
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetNonExistant()
        {
            var repository = new DatabaseScoreRepository(ConnectionFactory);

            var result = repository.GetById(1);
            Assert.Null(result);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterCreate()
        {
            var repository = new DatabaseScoreRepository(ConnectionFactory);
            var score = new Score(
                0,
                DataFixtures.GetPerson(ConnectionFactory), DataFixtures.GetClub(ConnectionFactory), DataFixtures.GetRound(ConnectionFactory), DataFixtures.GetCompetition(ConnectionFactory),
                Bowstyle.Barebow, 100, 10, 50, new DateTime(2017, 1, 1), new DateTime(2017, 2, 2),
                new[] { new ScoreTarget(0, 200, 75, 30) }
            );

            var insertResult = repository.Create(score);
            Assert.Equal(score, insertResult, new Score.EqualWithoutId());

            var getResult = repository.GetById(insertResult.Id);
            Assert.Equal(insertResult, getResult, new Score.EqualWithoutId());
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetWithNoTargets()
        {
            var repository = new DatabaseScoreRepository(ConnectionFactory);
            var score = new Score(
                0,
                DataFixtures.GetPerson(ConnectionFactory), DataFixtures.GetClub(ConnectionFactory), DataFixtures.GetRound(ConnectionFactory), DataFixtures.GetCompetition(ConnectionFactory),
                Bowstyle.Barebow, 100, 10, 50, new DateTime(2017, 1, 1), new DateTime(2017, 2, 2),
                new ScoreTarget[0]
            );

            var insertResult = repository.Create(score);
            Assert.Equal(score, insertResult, new Score.EqualWithoutId());

            var getResult = repository.GetById(insertResult.Id);
            Assert.Equal(insertResult, getResult, new Score.EqualWithoutId());
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllEmpty()
        {
            var repository = new DatabaseScoreRepository(ConnectionFactory);

            var getAllResult = repository.GetAll();
            Assert.Equal(0, getAllResult.Count);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllAfterInsert()
        {
            var repository = new DatabaseScoreRepository(ConnectionFactory);

            var score1 = new Score(
                0,
                DataFixtures.GetPerson(ConnectionFactory), DataFixtures.GetClub(ConnectionFactory), DataFixtures.GetRound(ConnectionFactory), DataFixtures.GetCompetition(ConnectionFactory),
                Bowstyle.Recurve, 120, 30, 60, new DateTime(2016, 1, 1), new DateTime(2016, 2, 2),
                new[] { new ScoreTarget(0, 300, 85, 40) }
            );
            var score2 = new Score(
                0,
                DataFixtures.GetPerson(ConnectionFactory), DataFixtures.GetClub(ConnectionFactory), DataFixtures.GetRound(ConnectionFactory), DataFixtures.GetCompetition(ConnectionFactory),
                Bowstyle.Barebow, 100, 10, 50, new DateTime(2017, 1, 1), new DateTime(2017, 2, 2),
                new[] { new ScoreTarget(0, 200, 75, 30) }
            );

            repository.Create(score1);
            repository.Create(score2);

            var getAllResult = repository.GetAll();
            Assert.Equal(2, getAllResult.Count);
            Assert.Equal(score1, getAllResult.First(x => x.PersonId == score1.PersonId), new Score.EqualWithoutId());
            Assert.Equal(score2, getAllResult.First(x => x.PersonId == score2.PersonId), new Score.EqualWithoutId());
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllByPersonId()
        {
            var repository = new DatabaseScoreRepository(ConnectionFactory);
            var person = DataFixtures.GetPerson(ConnectionFactory);
            var score1 = new Score(
                0,
                person, DataFixtures.GetClub(ConnectionFactory), DataFixtures.GetRound(ConnectionFactory), DataFixtures.GetCompetition(ConnectionFactory),
                Bowstyle.Recurve, 120, 30, 60, new DateTime(2016, 1, 1), new DateTime(2016, 2, 2),
                new[] { new ScoreTarget(0, 300, 85, 40) }
            );
            var score2 = new Score(
                0,
                DataFixtures.GetPerson(ConnectionFactory), DataFixtures.GetClub(ConnectionFactory), DataFixtures.GetRound(ConnectionFactory), DataFixtures.GetCompetition(ConnectionFactory),
                Bowstyle.Recurve, 120, 30, 60, new DateTime(2016, 1, 1), new DateTime(2016, 2, 2),
                new[] { new ScoreTarget(0, 300, 85, 40) }
            );

            var insertResult = repository.Create(score1);
            repository.Create(score2);

            var getAllResult = repository.GetAllByPersonId(person);
            Assert.Equal(1, getAllResult.Count);
            Assert.Equal(insertResult.Id, getAllResult.First().Id);
            Assert.Equal(insertResult, getAllResult.First(), new Score.EqualWithoutId());
        }
        [Fact, Trait("Type", "Integration")]
        public void TestGetAllByCompetitionId()
        {
            var repository = new DatabaseScoreRepository(ConnectionFactory);
            var competition = DataFixtures.GetCompetition(ConnectionFactory);
            var score1 = new Score(
                0,
                DataFixtures.GetPerson(ConnectionFactory), DataFixtures.GetClub(ConnectionFactory), DataFixtures.GetRound(ConnectionFactory), competition,
                Bowstyle.Recurve, 120, 30, 60, new DateTime(2016, 1, 1), new DateTime(2016, 2, 2),
                new[] { new ScoreTarget(0, 300, 85, 40) }
            );
            var score2 = new Score(
                0,
                DataFixtures.GetPerson(ConnectionFactory), DataFixtures.GetClub(ConnectionFactory), DataFixtures.GetRound(ConnectionFactory), DataFixtures.GetCompetition(ConnectionFactory),
                Bowstyle.Recurve, 120, 30, 60, new DateTime(2016, 1, 1), new DateTime(2016, 2, 2),
                new[] { new ScoreTarget(0, 300, 85, 40) }
            );
            var score3 = new Score(
                0,
                DataFixtures.GetPerson(ConnectionFactory), DataFixtures.GetClub(ConnectionFactory), DataFixtures.GetRound(ConnectionFactory), null,
                Bowstyle.Recurve, 120, 30, 60, new DateTime(2016, 1, 1), new DateTime(2016, 2, 2),
                new[] { new ScoreTarget(0, 300, 85, 40) }
            );

            var insertResult = repository.Create(score1);
            repository.Create(score2);
            repository.Create(score3);

            var getAllResult = repository.GetAllByCompetitionId(competition);
            Assert.Equal(1, getAllResult.Count);
            Assert.Equal(insertResult.Id, getAllResult.First().Id);
            Assert.Equal(insertResult, getAllResult.First(), new Score.EqualWithoutId());
        }
        [Fact, Trait("Type", "Integration")]
        public void TestGetAllByRoundId()
        {
            var repository = new DatabaseScoreRepository(ConnectionFactory);
            var round = DataFixtures.GetRound(ConnectionFactory);
            var score1 = new Score(
                0,
                DataFixtures.GetPerson(ConnectionFactory), DataFixtures.GetClub(ConnectionFactory), round, DataFixtures.GetCompetition(ConnectionFactory),
                Bowstyle.Recurve, 120, 30, 60, new DateTime(2016, 1, 1), new DateTime(2016, 2, 2),
                new[] { new ScoreTarget(0, 300, 85, 40) }
            );
            var score2 = new Score(
                0,
                DataFixtures.GetPerson(ConnectionFactory), DataFixtures.GetClub(ConnectionFactory), DataFixtures.GetRound(ConnectionFactory), DataFixtures.GetCompetition(ConnectionFactory),
                Bowstyle.Recurve, 120, 30, 60, new DateTime(2016, 1, 1), new DateTime(2016, 2, 2),
                new[] { new ScoreTarget(0, 300, 85, 40) }
            );

            var insertResult = repository.Create(score1);
            repository.Create(score2);

            var getAllResult = repository.GetAllByRoundId(round);
            Assert.Equal(1, getAllResult.Count);
            Assert.Equal(insertResult.Id, getAllResult.First().Id);
            Assert.Equal(insertResult, getAllResult.First(), new Score.EqualWithoutId());
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterUpdate()
        {
            var repository = new DatabaseScoreRepository(ConnectionFactory);
            var score = new Score(
                0,
                DataFixtures.GetPerson(ConnectionFactory), DataFixtures.GetClub(ConnectionFactory), DataFixtures.GetRound(ConnectionFactory), DataFixtures.GetCompetition(ConnectionFactory),
                Bowstyle.Recurve, 120, 30, 60, new DateTime(2016, 1, 1), new DateTime(2016, 2, 2),
                new[] { new ScoreTarget(0, 300, 85, 40) }
            );
            var insertResult = repository.Create(score);

            var updatedScore = new Score(
                insertResult.Id,
                DataFixtures.GetPerson(ConnectionFactory), DataFixtures.GetClub(ConnectionFactory), DataFixtures.GetRound(ConnectionFactory), DataFixtures.GetCompetition(ConnectionFactory),
                Bowstyle.Barebow, 100, 10, 50, new DateTime(2017, 1, 1), new DateTime(2017, 2, 2),
                new[] { new ScoreTarget(0, 200, 75, 30) }
            );
            var updateResult = repository.Update(updatedScore);

            Assert.Equal(updatedScore, updateResult, new Score.EqualWithoutId());

            var getResult = repository.GetById(updateResult.Id);
            Assert.Equal(updateResult, getResult, new Score.EqualWithoutId());
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterRemove()
        {
            var repository = new DatabaseScoreRepository(ConnectionFactory);
            var score = new Score(
                0,
                DataFixtures.GetPerson(ConnectionFactory), DataFixtures.GetClub(ConnectionFactory), DataFixtures.GetRound(ConnectionFactory), DataFixtures.GetCompetition(ConnectionFactory),
                Bowstyle.Recurve, 120, 30, 60, new DateTime(2016, 1, 1), new DateTime(2016, 2, 2),
                new[] { new ScoreTarget(0, 300, 85, 40) }
            );
            var insertResult = repository.Create(score);

            var removeResult = repository.Delete(insertResult);
            Assert.True(removeResult);

            var result = repository.GetById(insertResult.Id);
            Assert.Null(result);
        }
    }
}

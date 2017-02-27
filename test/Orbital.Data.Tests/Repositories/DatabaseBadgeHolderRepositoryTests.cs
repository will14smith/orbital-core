using Orbital.Data.Repositories;
using Orbital.Data.Tests.Helpers;
using Orbital.Models.Domain;
using System;
using System.Linq;
using Xunit;

namespace Orbital.Data.Tests.Repositories
{
    public class DatabaseBadgeHolderRepositoryTests : DatabaseRepositoryTestBase
    {
        public DatabaseBadgeHolderRepositoryTests(DatabaseFixture databaseFixture) : base(databaseFixture)
        {
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetNonExistant()
        {
            var repository = new DatabaseBadgeHolderRepository(ConnectionFactory);

            var result = repository.GetById(1);
            Assert.Null(result);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterCreate()
        {
            var repository = new DatabaseBadgeHolderRepository(ConnectionFactory);
            var holder = new BadgeHolder(0, DataFixtures.GetBadge(ConnectionFactory), DataFixtures.GetPerson(ConnectionFactory), new DateTime(2017, 1, 1), new DateTime(2017, 2, 2), new DateTime(2017, 3, 3), new DateTime(2017, 4, 4));

            var insertResult = repository.Create(holder);
            AssertEqualNotId(holder, insertResult);

            var getResult = repository.GetById(insertResult.Id);
            Assert.Equal(insertResult, getResult);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllEmpty()
        {
            var repository = new DatabaseBadgeHolderRepository(ConnectionFactory);

            var getAllResult = repository.GetAll();
            Assert.Equal(0, getAllResult.Count);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllAfterInsert()
        {
            var repository = new DatabaseBadgeHolderRepository(ConnectionFactory);
            var holder = new BadgeHolder(0, DataFixtures.GetBadge(ConnectionFactory), DataFixtures.GetPerson(ConnectionFactory), new DateTime(2017, 1, 1), new DateTime(2017, 2, 2), new DateTime(2017, 3, 3), new DateTime(2017, 4, 4));
            var insertResult = repository.Create(holder);

            var getAllResult = repository.GetAll();
            Assert.Equal(1, getAllResult.Count);
            Assert.Equal(insertResult, getAllResult.First());
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllByBadgeEmpty()
        {
            var repository = new DatabaseBadgeHolderRepository(ConnectionFactory);
            var badge = DataFixtures.GetBadge(ConnectionFactory);

            var getAllResult = repository.GetAllByBadgeId(badge);
            Assert.Equal(0, getAllResult.Count);
        }
        [Fact, Trait("Type", "Integration")]
        public void TestGetAllByBadgeAfterInsert()
        {
            var repository = new DatabaseBadgeHolderRepository(ConnectionFactory);
            var badge = DataFixtures.GetBadge(ConnectionFactory);
            var holder = new BadgeHolder(0, badge, DataFixtures.GetPerson(ConnectionFactory), new DateTime(2017, 1, 1), new DateTime(2017, 2, 2), new DateTime(2017, 3, 3), new DateTime(2017, 4, 4));
            var insertResult = repository.Create(holder);

            var getAllResult = repository.GetAllByBadgeId(badge);
            Assert.Equal(1, getAllResult.Count);
            Assert.Equal(insertResult, getAllResult.First());
        }
        [Fact, Trait("Type", "Integration")]
        public void TestGetAllByBadgeAfterInsertEmpty()
        {
            var repository = new DatabaseBadgeHolderRepository(ConnectionFactory);
            var badge1 = DataFixtures.GetBadge(ConnectionFactory);
            var badge2 = DataFixtures.GetBadge(ConnectionFactory);
            var holder = new BadgeHolder(0, badge1, DataFixtures.GetPerson(ConnectionFactory), new DateTime(2017, 1, 1), new DateTime(2017, 2, 2), new DateTime(2017, 3, 3), new DateTime(2017, 4, 4));
            repository.Create(holder);

            var getAllResult = repository.GetAllByBadgeId(badge2);
            Assert.Equal(0, getAllResult.Count);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAllByPersonEmpty()
        {
            var repository = new DatabaseBadgeHolderRepository(ConnectionFactory);
            var person = DataFixtures.GetPerson(ConnectionFactory);

            var getAllResult = repository.GetAllByPersonId(person);
            Assert.Equal(0, getAllResult.Count);
        }
        [Fact, Trait("Type", "Integration")]
        public void TestGetAllByPersonAfterInsert()
        {
            var repository = new DatabaseBadgeHolderRepository(ConnectionFactory);
            var person = DataFixtures.GetPerson(ConnectionFactory);
            var holder = new BadgeHolder(0, DataFixtures.GetBadge(ConnectionFactory), person, new DateTime(2017, 1, 1), new DateTime(2017, 2, 2), new DateTime(2017, 3, 3), new DateTime(2017, 4, 4));
            var insertResult = repository.Create(holder);

            var getAllResult = repository.GetAllByPersonId(person);
            Assert.Equal(1, getAllResult.Count);
            Assert.Equal(insertResult, getAllResult.First());
        }
        [Fact, Trait("Type", "Integration")]
        public void TestGetAllByPersonAfterInsertEmpty()
        {
            var repository = new DatabaseBadgeHolderRepository(ConnectionFactory);
            var person1 = DataFixtures.GetPerson(ConnectionFactory);
            var person2 = DataFixtures.GetPerson(ConnectionFactory);
            var holder = new BadgeHolder(0, DataFixtures.GetBadge(ConnectionFactory), person1, new DateTime(2017, 1, 1), new DateTime(2017, 2, 2), new DateTime(2017, 3, 3), new DateTime(2017, 4, 4));
            repository.Create(holder);

            var getAllResult = repository.GetAllByPersonId(person2);
            Assert.Equal(0, getAllResult.Count);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterUpdate()
        {
            var badge1 = DataFixtures.GetBadge(ConnectionFactory);
            var badge2 = DataFixtures.GetBadge(ConnectionFactory);
            var person1 = DataFixtures.GetPerson(ConnectionFactory);
            var person2 = DataFixtures.GetPerson(ConnectionFactory);

            var repository = new DatabaseBadgeHolderRepository(ConnectionFactory);
            var holder = new BadgeHolder(0, badge1, person1, new DateTime(2017, 1, 1), new DateTime(2017, 2, 2), new DateTime(2017, 3, 3), new DateTime(2017, 4, 4));
            var insertResult = repository.Create(holder);

            var updatedHolder = new BadgeHolder(insertResult.Id, badge2, person2, new DateTime(2018, 1, 1), new DateTime(2018, 2, 2), new DateTime(2018, 3, 3), new DateTime(2018, 4, 4));
            var updateResult = repository.Update(updatedHolder);

            AssertEqualNotId(updatedHolder, updateResult);

            var getResult = repository.GetById(updateResult.Id);
            Assert.Equal(updateResult, getResult);
        }

        [Fact, Trait("Type", "Integration")]
        public void TestGetAfterRemove()
        {
            var repository = new DatabaseBadgeHolderRepository(ConnectionFactory);
            var holder = new BadgeHolder(0, DataFixtures.GetBadge(ConnectionFactory), DataFixtures.GetPerson(ConnectionFactory), new DateTime(2017, 1, 1), new DateTime(2017, 2, 2), new DateTime(2017, 3, 3), new DateTime(2017, 4, 4));
            var insertResult = repository.Create(holder);

            var removeResult = repository.Delete(insertResult);
            Assert.True(removeResult);

            var result = repository.GetById(insertResult.Id);
            Assert.Null(result);
        }

        private static void AssertEqualNotId(BadgeHolder expected, BadgeHolder actual)
        {
            Assert.Equal(expected.BadgeId, actual.BadgeId);
            Assert.Equal(expected.PersonId, actual.PersonId);
            Assert.Equal(expected.AwardedOn, actual.AwardedOn);
            Assert.Equal(expected.ConfirmedOn, actual.ConfirmedOn);
            Assert.Equal(expected.MadeOn, actual.MadeOn);
            Assert.Equal(expected.DeliveredOn, actual.DeliveredOn);
        }
    }
}

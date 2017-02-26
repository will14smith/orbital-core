using System;
using Orbital.Data.Repositories;
using Orbital.Models.Domain;
using Xunit;

namespace Orbital.Data.Tests.Repositories
{
    public class InMemoryBadgeHolderRepositoryTests
    {
        [Fact]
        public void TestGetAllByBadgeId()
        {
            var holders = new[]
            {
                new BadgeHolder(1, 2, 3, DateTime.Now, null, null, null),
                new BadgeHolder(2, 3, 3, DateTime.Now, null, null, null),
            };

            var repo = InMemoryBadgeHolderRepository.New(holders);

            var result1 = repo.GetAllByBadgeId(1);
            Assert.Empty(result1);

            var result2 = repo.GetAllByBadgeId(2);
            Assert.Equal(new[] { holders[0] }, result2);
        }

        [Fact]
        public void TestGetAllByBadgeIdEmpty()
        {
            var repo = InMemoryBadgeHolderRepository.New();

            var result = repo.GetAllByBadgeId(1);
            Assert.Empty(result);
        }

        [Fact]
        public void TestGetAllByPersonId()
        {
            var holders = new[]
            {
                new BadgeHolder(1, 2, 3, DateTime.Now, null, null, null),
                new BadgeHolder(2, 2, 4, DateTime.Now, null, null, null),
            };

            var repo = InMemoryBadgeHolderRepository.New(holders);

            var result1 = repo.GetAllByPersonId(2);
            Assert.Empty(result1);

            var result2 = repo.GetAllByPersonId(3);
            Assert.Equal(new[] { holders[0] }, result2);
        }

        [Fact]
        public void TestGetAllByPersonIdEmpty()
        {
            var repo = InMemoryBadgeHolderRepository.New();

            var result = repo.GetAllByPersonId(1);
            Assert.Empty(result);
        }
    }
}

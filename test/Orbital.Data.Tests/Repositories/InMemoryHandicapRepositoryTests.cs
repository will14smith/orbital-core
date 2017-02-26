using System;
using System.Linq;
using Orbital.Data.Repositories;
using Orbital.Models.Domain;
using Xunit;

namespace Orbital.Data.Tests.Repositories
{
    public class InMemoryHandicapRepositoryTests
    {
        [Fact]
        public void TestGetAllByPersonIdOnlyCorrect()
        {
            var handicaps = new[]
            {
                new Handicap(1, 1, null, HandicapType.Initial, DateTime.Now, 1, new HandicapIdentifier(true, Bowstyle.Recurve)),
                new Handicap(2, 2, null, HandicapType.Initial, DateTime.Now, 1, new HandicapIdentifier(true, Bowstyle.Recurve)),
            };

            var repo = InMemoryHandicapRepository.New(handicaps);

            var result = repo.GetAllByPersonId(1);

            Assert.Equal(1, result.Count);
            Assert.Equal(new[] { handicaps[0] }, result.First());
        }
        [Fact]
        public void TestGetAllByPersonIdNotFound()
        {
            var repo = InMemoryHandicapRepository.New();

            var result = repo.GetAllByPersonId(1);

            Assert.Empty(result);
        }
        [Fact]
        public void TestGetAllByPersonIdGrouping()
        {
            var handicaps = new[]
            {
                new Handicap(1, 1, null, HandicapType.Initial, DateTime.Now, 1, new HandicapIdentifier(true, Bowstyle.Recurve)),
                new Handicap(2, 1, null, HandicapType.Initial, DateTime.Now, 2, new HandicapIdentifier(true, Bowstyle.Recurve)),
                new Handicap(3, 1, null, HandicapType.Initial, DateTime.Now, 3, new HandicapIdentifier(false, Bowstyle.Recurve)),
                new Handicap(4, 1, null, HandicapType.Initial, DateTime.Now, 3, new HandicapIdentifier(true, Bowstyle.Barebow)),
            };

            var repo = InMemoryHandicapRepository.New(handicaps);

            var result = repo.GetAllByPersonId(1);

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void TestGetLatestByPersonIdNotFound()
        {
            var repo = InMemoryHandicapRepository.New();

            var result = repo.GetLatestByPersonId(1);

            Assert.Empty(result);
        }
        [Fact]
        public void TestGetLatestByPersonIdOnlyLatestPerGroup()
        {
            var group1 = new HandicapIdentifier(true, Bowstyle.Recurve);
            var group2 = new HandicapIdentifier(false, Bowstyle.Recurve);

            var handicaps = new[]
            {
                new Handicap(1, 1, null, HandicapType.Initial, DateTime.Now, 1, group1),
                new Handicap(2, 1, null, HandicapType.Initial, DateTime.Now.AddDays(1), 2, group1),

                new Handicap(3, 1, null, HandicapType.Initial, DateTime.Now.AddDays(2), 1, group2),
                new Handicap(4, 1, null, HandicapType.Initial, DateTime.Now.AddDays(3), 2, group2),
            };

            var repo = InMemoryHandicapRepository.New(handicaps);

            var result = repo.GetLatestByPersonId(1);

            Assert.Equal(2, result.Count);
            Assert.Equal(handicaps[1], result[group1]);
            Assert.Equal(handicaps[3], result[group2]);
        }

        [Fact]
        public void TestGetLatestByPersonIdPersonNotFound()
        {
            var group1 = new HandicapIdentifier(true, Bowstyle.Recurve);

            var handicaps = new[]
            {
                new Handicap(1, 2, null, HandicapType.Initial, DateTime.Now, 1, group1),
            };

            var repo = InMemoryHandicapRepository.New(handicaps);

            var result = repo.GetLatestByPersonId(group1, 1);

            Assert.Null(result);
        }
        [Fact]
        public void TestGetLatestByPersonIdHandicapNotFound()
        {
            var group1 = new HandicapIdentifier(true, Bowstyle.Recurve);
            var group2 = new HandicapIdentifier(false, Bowstyle.Recurve);

            var handicaps = new[]
            {
                new Handicap(1, 1, null, HandicapType.Initial, DateTime.Now, 1, group2),
            };

            var repo = InMemoryHandicapRepository.New(handicaps);

            var result = repo.GetLatestByPersonId(group1, 1);

            Assert.Null(result);
        }
        [Fact]
        public void TestGetLatestByPersonIdCorrectGroupAndLatest()
        {
            var group1 = new HandicapIdentifier(true, Bowstyle.Recurve);
            var group2 = new HandicapIdentifier(false, Bowstyle.Recurve);

            var handicaps = new[]
            {
                new Handicap(1, 1, null, HandicapType.Initial, DateTime.Now, 1, group1),
                new Handicap(2, 1, null, HandicapType.Initial, DateTime.Now.AddDays(1), 2, group1),
                new Handicap(3, 1, null, HandicapType.Initial, DateTime.Now.AddDays(2), 1, group2),
            };

            var repo = InMemoryHandicapRepository.New(handicaps);

            var result = repo.GetLatestByPersonId(group1, 1);

            Assert.Equal(handicaps[1], result);
        }

        [Fact]
        public void TestGetByScoreIdOnlyCorrect()
        {
            var handicaps = new[]
            {
                new Handicap(1, 1, null, HandicapType.Initial, DateTime.Now, 1, new HandicapIdentifier(true, Bowstyle.Recurve)),
                new Handicap(2, 1, 1, HandicapType.Initial, DateTime.Now, 1, new HandicapIdentifier(true, Bowstyle.Recurve)),
                new Handicap(3, 2, 2, HandicapType.Initial, DateTime.Now, 1, new HandicapIdentifier(true, Bowstyle.Recurve)),
            };

            var repo = InMemoryHandicapRepository.New(handicaps);

            var result = repo.GetByScoreId(1);

            Assert.Equal(handicaps[1], result);
        }
        [Fact]
        public void TestGetByScoreIdNotFound()
        {
            var repo = InMemoryHandicapRepository.New();

            var result = repo.GetByScoreId(1);

            Assert.Null(result);
        }
        [Fact]
        public void TestGetByScoreIdDuplicate()
        {
            var handicaps = new[]
            {
                new Handicap(1, 1, 1, HandicapType.Initial, DateTime.Now, 1, new HandicapIdentifier(true, Bowstyle.Recurve)),
                new Handicap(2, 1, 1, HandicapType.Initial, DateTime.Now, 1, new HandicapIdentifier(true, Bowstyle.Recurve)),
            };

            var repo = InMemoryHandicapRepository.New(handicaps);

            Assert.Throws<InvalidOperationException>(() => repo.GetByScoreId(1));
        }
    }
}

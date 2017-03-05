using System;
using Orbital.Data.Repositories;
using Orbital.Models.Domain;
using Xunit;
using Record = Orbital.Models.Domain.Record;

namespace Orbital.Data.Tests.Repositories
{
    public class InMemoryRecordRepositoryTests
    {
        [Fact]
        public void TestGetAllByClubId()
        {
            var records = new[]
            {
                new Record(1, 1, new [] { new RecordClub(2, DateTime.Now, DateTime.Now) }, null),
                new Record(2, 1, new [] { new RecordClub(3, DateTime.Now, DateTime.Now) }, null),
            };

            var repo = InMemoryRecordRepository.New(records);

            var result1 = repo.GetAllByClubId(1);
            Assert.Empty(result1);

            var result2 = repo.GetAllByClubId(2);
            Assert.Equal(new[] { records[0] }, result2);
        }

        [Fact]
        public void TestGetAllByClubIdMultiple()
        {
            var records = new[]
            {
                new Record(1, 1, new [] { new RecordClub(4, DateTime.Now, DateTime.Now), new RecordClub(2, DateTime.Now, DateTime.Now) }, null),
                new Record(2, 1, new [] { new RecordClub(4, DateTime.Now, DateTime.Now), new RecordClub(3, DateTime.Now, DateTime.Now) }, null),
            };

            var repo = InMemoryRecordRepository.New(records);

            var result1 = repo.GetAllByClubId(1);
            Assert.Empty(result1);

            var result2 = repo.GetAllByClubId(2);
            Assert.Equal(new[] { records[0] }, result2);
        }

        [Fact]
        public void TestGetAllByClubIdNull()
        {
            var records = new[]
            {
                new Record(1, 1, null, null),
            };

            var repo = InMemoryRecordRepository.New(records);

            var result = repo.GetAllByClubId(1);
            Assert.Empty(result);
        }

        [Fact]
        public void TestGetAllByClubIdEmpty()
        {
            var repo = InMemoryRecordRepository.New();

            var result = repo.GetAllByClubId(1);
            Assert.Empty(result);
        }
        
        [Fact]
        public void TestGetAllByRoundId()
        {
            var records = new[]
            {
                new Record(1, 1, null, new [] { new RecordRound(2, 1, null, null, null) }),
                new Record(2, 1, null, new [] { new RecordRound(3, 1, null, null, null) }),
            };

            var repo = InMemoryRecordRepository.New(records);

            var result1 = repo.GetAllByRoundId(1);
            Assert.Empty(result1);

            var result2 = repo.GetAllByRoundId(2);
            Assert.Equal(new[] { records[0] }, result2);
        }

        [Fact]
        public void TestGetAllByRoundIdMultiple()
        {
            var records = new[]
            {
                new Record(1, 1, null, new [] { new RecordRound(4, 1, null, null, null), new RecordRound(2, 1, null, null, null) }),
                new Record(2, 1, null, new [] { new RecordRound(4, 1, null, null, null), new RecordRound(3, 1, null, null, null) }),
            };

            var repo = InMemoryRecordRepository.New(records);

            var result1 = repo.GetAllByRoundId(1);
            Assert.Empty(result1);

            var result2 = repo.GetAllByRoundId(2);
            Assert.Equal(new[] { records[0] }, result2);
        }

        [Fact]
        public void TestGetAllByRoundIdNull()
        {
            var records = new[]
            {
                new Record(1, 1, null, null),
            };

            var repo = InMemoryRecordRepository.New(records);

            var result = repo.GetAllByRoundId(1);
            Assert.Empty(result);
        }

        [Fact]
        public void TestGetAllByRoundIdEmpty()
        {
            var repo = InMemoryRecordRepository.New();

            var result = repo.GetAllByRoundId(1);
            Assert.Empty(result);
        }
    }
}

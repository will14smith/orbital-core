using System;
using Orbital.Data.Repositories;
using Orbital.Models.Domain;
using Xunit;

namespace Orbital.Data.Tests.Repositories
{
    public class InMemoryRecordTeamRepositoryTests
    {
        [Fact]
        public void TestGetAllByRecordIdOnlyCorrect()
        {
            var teams = new[]
            {
                new RecordTeam(1, 2, 1, 1, DateTime.Now, null, null, null, null),
                new RecordTeam(1, 3, 1, 1, DateTime.Now, null, null, null, null)
            };

            var repo = InMemoryRecordTeamRepository.New(teams);

            var result = repo.GetAllByRecordId(2);

            Assert.Equal(new[] { teams[0] }, result);
        }
        [Fact]
        public void TestGetAllByRecordIdNotFound()
        {
            var repo = InMemoryRecordTeamRepository.New();

            var result = repo.GetAllByRecordId(1);

            Assert.Empty(result);
        }

        [Fact]
        public void TestGetAllByCompetitionIdOnlyCorrect()
        {
            var teams = new[]
            {
                new RecordTeam(1, 1, 1, 2, DateTime.Now, null, null, null, null),
                new RecordTeam(1, 1, 1, 3, DateTime.Now, null, null, null, null)
            };

            var repo = InMemoryRecordTeamRepository.New(teams);

            var result = repo.GetAllByCompetitionId(2);

            Assert.Equal(new[] { teams[0] }, result);
        }
        [Fact]
        public void TestGetAllByCompetitionIdNotFound()
        {
            var repo = InMemoryRecordTeamRepository.New();

            var result = repo.GetAllByCompetitionId(1);

            Assert.Empty(result);
        }

        [Fact]
        public void TestGetAllByPersonIdOnlyCorrect()
        {
            var teams = new[]
            {
                new RecordTeam(1, 1, 1, 1, DateTime.Now, null, null, null, new [] { new RecordTeamMember(1, 4, null, 1), new RecordTeamMember(1, 2, null, 1),  }),
                new RecordTeam(1, 1, 1, 1, DateTime.Now, null, null, null, new [] { new RecordTeamMember(1, 4, null, 1)  })
            };

            var repo = InMemoryRecordTeamRepository.New(teams);

            var result = repo.GetAllByPersonId(2);

            Assert.Equal(new[] { teams[0] }, result);
        }
        [Fact]
        public void TestGetAllByPersonIdNotFound()
        {
            var repo = InMemoryRecordTeamRepository.New();

            var result = repo.GetAllByPersonId(1);

            Assert.Empty(result);
        }

        [Fact]
        public void TestGetAllByScoreIdOnlyCorrect()
        {
            var teams = new[]
            {
                new RecordTeam(1, 1, 1, 1, DateTime.Now, null, null, null, new [] { new RecordTeamMember(1, 1, null, 1), new RecordTeamMember(1, 1, 2, 1),  }),
                new RecordTeam(1, 1, 1, 1, DateTime.Now, null, null, null, new [] { new RecordTeamMember(1, 1, 3, 1) })
            };

            var repo = InMemoryRecordTeamRepository.New(teams);

            var result = repo.GetAllByScoreId(2);

            Assert.Equal(new[] { teams[0] }, result);
        }
        [Fact]
        public void TestGetAllByScoreIdNotFound()
        {
            var repo = InMemoryRecordTeamRepository.New();

            var result = repo.GetAllByScoreId(1);

            Assert.Empty(result);
        }

        [Fact]
        public void TestGetLatestByRecordIdOnlyCorrect()
        {
            var teams = new[]
            {
                new RecordTeam(1, 2, 1, 1, DateTime.Now, null, null, null, null),
                new RecordTeam(1, 2, 1, 1, DateTime.Now.AddDays(1), null, null, null, null),
                new RecordTeam(1, 3, 1, 1, DateTime.Now.AddDays(2), null, null, null, null),
            };

            var repo = InMemoryRecordTeamRepository.New(teams);

            var result = repo.GetLatestByRecordId(2);

            Assert.Equal(teams[1], result);
        }
        [Fact]
        public void TestGetLatestByRecordIdNotFound()
        {
            var repo = InMemoryRecordTeamRepository.New();

            var result = repo.GetLatestByRecordId(1);

            Assert.Null(result);
        }
    }
}

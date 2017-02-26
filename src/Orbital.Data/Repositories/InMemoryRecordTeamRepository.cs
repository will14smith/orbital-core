using System.Collections.Generic;
using System.Linq;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Data.Repositories
{
    public class InMemoryRecordTeamRepository : InMemoryRepository<RecordTeam>, IRecordTeamRepository
    {
        private InMemoryRecordTeamRepository(List<RecordTeam> data) : base(data) { }

        public static InMemoryRecordTeamRepository New(params RecordTeam[] teams)
        {
            return new InMemoryRecordTeamRepository(teams.ToList());
        }

        public IReadOnlyCollection<RecordTeam> GetAllByRecordId(int recordId) => Data.Where(x => x.RecordId == recordId).ToList();
        public IReadOnlyCollection<RecordTeam> GetAllByCompetitionId(int competitionId) => Data.Where(x => x.CompetitionId == competitionId).ToList();
        public IReadOnlyCollection<RecordTeam> GetAllByPersonId(int personId) => Data.Where(x => x.Members.Any(m => m.PersonId == personId)).ToList();
        public IReadOnlyCollection<RecordTeam> GetAllByScoreId(int scoreId) => Data.Where(x => x.Members.Any(m => m.ScoreId == scoreId)).ToList();

        public RecordTeam GetLatestByRecordId(int recordId) => Data.Where(x => x.RecordId == recordId).OrderByDescending(x => x.DateSet).FirstOrDefault();

        protected override int GetId(RecordTeam item) => item.Id;
    }
}
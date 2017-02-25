using System.Collections.Generic;
using Orbital.Models.Domain;

namespace Orbital.Models.Repositories
{
    public interface IRecordTeamRepository : IRepository<RecordTeam>
    {
        IReadOnlyCollection<RecordTeam> GetAllByRecordId(int recordId);
        IReadOnlyCollection<RecordTeam> GetAllByCompetitionId(int competitionId);
        IReadOnlyCollection<RecordTeam> GetAllByPersonId(int personId);
        IReadOnlyCollection<RecordTeam> GetAllByScoreId(int scoreId);

        RecordTeam GetLatestByRecordId(int recordId);
    }
}
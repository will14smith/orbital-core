using Orbital.Models.Domain;
using System.Collections.Generic;

namespace Orbital.Models.Repositories
{
    public interface IScoreRepository : IRepository<Score>
    {
        IReadOnlyCollection<Score> GetAllByPersonId(int personId);
        IReadOnlyCollection<Score> GetAllByRoundId(int roundId);
        IReadOnlyCollection<Score> GetAllByCompetitionId(int competitionId);
    }
}

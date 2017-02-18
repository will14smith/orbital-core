using Orbital.Models.Domain;
using System.Collections.Generic;

namespace Orbital.Models.Repositories
{
    public interface IScoreRepository
    {
        IReadOnlyCollection<Score> GetAll();
        IReadOnlyCollection<Score> GetAllByPersonId(int personId);
        IReadOnlyCollection<Score> GetAllByRoundId(int roundId);
        IReadOnlyCollection<Score> GetAllByCompetitionId(int competitionId);
        Score GetById(int id);

        Score Create(Score round);
        Score Update(Score round);
        bool Delete(Score round);
    }
}

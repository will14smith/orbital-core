using System.Collections.Generic;
using System.Linq;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Data.Repositories
{
    public class InMemoryScoreRepository : InMemoryRepository<Score>, IScoreRepository
    {
        private InMemoryScoreRepository(List<Score> data) : base(data) { }

        public static InMemoryScoreRepository New(params Score[] scores)
        {
            return new InMemoryScoreRepository(scores.ToList());
        }

        public IReadOnlyCollection<Score> GetAllByPersonId(int personId) => Data.Where(x => x.PersonId == personId).ToList();
        public IReadOnlyCollection<Score> GetAllByRoundId(int roundId) => Data.Where(x => x.RoundId == roundId).ToList();
        public IReadOnlyCollection<Score> GetAllByCompetitionId(int competitionId) => Data.Where(x => x.CompetitionId.HasValue && x.CompetitionId.Value == competitionId).ToList();

        protected override int GetId(Score item) => item.Id;
    }
}

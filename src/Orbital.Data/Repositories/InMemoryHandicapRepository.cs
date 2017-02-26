using System.Collections.Generic;
using System.Linq;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Data.Repositories
{
    public class InMemoryHandicapRepository : InMemoryRepository<Handicap>, IHandicapRepository
    {
        private InMemoryHandicapRepository(List<Handicap> data) : base(data) { }

        public static InMemoryHandicapRepository New(params Handicap[] handicaps)
        {
            return new InMemoryHandicapRepository(handicaps.ToList());
        }

        public ILookup<HandicapIdentifier, Handicap> GetAllByPersonId(int personId) => Data.Where(x => x.PersonId == personId).ToLookup(x => x.Identifier);

        public IReadOnlyDictionary<HandicapIdentifier, Handicap> GetLatestByPersonId(int personId) => GetAllByPersonId(personId).ToDictionary(x => x.Key, x => x.OrderByDescending(h => h.Date).First());
        public Handicap GetLatestByPersonId(HandicapIdentifier id, int personId) => GetLatestByPersonId(personId).SafeGet(id);

        public Handicap GetByScoreId(int scoreId) => Data.SingleOrDefault(x => x.ScoreId.HasValue && x.ScoreId.Value == scoreId);

        protected override int GetId(Handicap item) => item.Id;
    }
}
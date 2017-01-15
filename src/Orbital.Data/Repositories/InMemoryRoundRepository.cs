using System.Collections.Generic;
using System.Linq;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Data.Repositories
{
    public class InMemoryRoundRepository : IRoundRepository
    {
        private List<Round> _rounds;

        public InMemoryRoundRepository()
        {
            _rounds = new List<Round>();
        }

        public static InMemoryRoundRepository New(params Round[] rounds)
        {
            return new InMemoryRoundRepository { _rounds = rounds.ToList() };
        }

        public IReadOnlyCollection<Round> GetAll() { return _rounds; }
        public IReadOnlyCollection<Round> GetAllVariantsById(int parentRoundId) { return _rounds.Where(x => x.VariantOfId == parentRoundId).ToList(); }
        public Round GetById(int id) { return _rounds.FirstOrDefault(x => x.Id == id); }

        public Round Create(Round round) { _rounds.Add(round); return round; }
        public Round Update(Round round) { Delete(round); return Create(round); }
        public bool Delete(Round round) { return _rounds.Remove(GetById(round.Id)); }

    }
}

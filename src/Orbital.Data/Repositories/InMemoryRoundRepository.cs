using System.Collections.Generic;
using System.Linq;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Data.Repositories
{
    public class InMemoryRoundRepository : InMemoryRepository<Round>, IRoundRepository
    {
        private InMemoryRoundRepository(List<Round> data) : base(data) { }

        public static InMemoryRoundRepository New(params Round[] rounds)
        {
            return new InMemoryRoundRepository(rounds.ToList());
        }

        public IReadOnlyCollection<Round> GetAllVariantsById(int parentRoundId) => Data.Where(x => x.VariantOfId == parentRoundId).ToList();

        protected override int GetId(Round item) => item.Id;
    }
}

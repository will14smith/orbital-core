using System.Collections.Generic;
using System.Linq;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Data.Repositories
{
    public class InMemoryBadgeHolderRepository : InMemoryRepository<BadgeHolder>, IBadgeHolderRepository
    {
        private InMemoryBadgeHolderRepository(List<BadgeHolder> data) : base(data) { }

        public static InMemoryBadgeHolderRepository New(params BadgeHolder[] holders)
        {
            return new InMemoryBadgeHolderRepository(holders.ToList());
        }

        public IReadOnlyCollection<BadgeHolder> GetAllByBadgeId(int badgeId) => Data.Where(x => x.BadgeId == badgeId).ToList();
        public IReadOnlyCollection<BadgeHolder> GetAllByPersonId(int personId) => Data.Where(x => x.PersonId == personId).ToList();

        protected override int GetId(BadgeHolder item) => item.Id;
    }
}

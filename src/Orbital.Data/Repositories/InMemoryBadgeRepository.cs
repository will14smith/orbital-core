using System.Collections.Generic;
using System.Linq;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Data.Repositories
{
    public class InMemoryBadgeRepository : InMemoryRepository<Badge>, IBadgeRepository
    {
        private InMemoryBadgeRepository(List<Badge> data) : base(data) { }

        public static InMemoryBadgeRepository New(params Badge[] badges)
        {
            return new InMemoryBadgeRepository(badges.ToList());
        }

        protected override int GetId(Badge item) => item.Id;

    }
}

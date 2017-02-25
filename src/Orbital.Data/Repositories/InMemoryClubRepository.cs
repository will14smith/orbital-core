using System.Collections.Generic;
using System.Linq;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Data.Repositories
{
    public class InMemoryClubRepository : InMemoryRepository<Club>, IClubRepository
    {
        private InMemoryClubRepository(List<Club> data) : base(data) { }

        public static InMemoryClubRepository New(params Club[] clubs)
        {
            return new InMemoryClubRepository(clubs.ToList());
        }

        protected override int GetId(Club item) => item.Id;
    }
}

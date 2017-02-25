using System.Collections.Generic;
using System.Linq;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Data.Repositories
{
    public class InMemoryCompetitionRepository : InMemoryRepository<Competition>, ICompetitionRepository
    {
        private InMemoryCompetitionRepository(List<Competition> data) : base(data) { }

        public static InMemoryCompetitionRepository New(params Competition[] competitions)
        {
            return new InMemoryCompetitionRepository(competitions.ToList());
        }

        protected override int GetId(Competition item) => item.Id;
    }
}

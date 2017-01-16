using System.Collections.Generic;
using System.Linq;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Data.Repositories
{
    public class InMemoryCompetitionRepository : ICompetitionRepository
    {
        private List<Competition> _competitions;

        public InMemoryCompetitionRepository()
        {
            _competitions = new List<Competition>();
        }

        public static InMemoryCompetitionRepository New(params Competition[] competitions)
        {
            return new InMemoryCompetitionRepository { _competitions = competitions.ToList() };
        }

        public IReadOnlyCollection<Competition> GetAll() { return _competitions; }
        public Competition GetById(int id) { return _competitions.FirstOrDefault(x => x.Id == id); }

        public Competition Create(Competition competition) { _competitions.Add(competition); return competition; }
        public Competition Update(Competition competition) { Delete(competition); return Create(competition); }
        public bool Delete(Competition competition) { return _competitions.Remove(GetById(competition.Id)); }
    }
}

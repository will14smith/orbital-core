using System.Collections.Generic;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Schema.Competitions
{
    public class CompetitionServiceImpl : ICompetitionService
    {
        private readonly ICompetitionRepository _competitionRepository;

        public CompetitionServiceImpl(ICompetitionRepository competitionRepository)
        {
            _competitionRepository = competitionRepository;
        }

        public IReadOnlyCollection<Competition> GetRoot()
        {
            return _competitionRepository.GetAll();
        }
        
        public Competition GetById(int id)
        {
            return _competitionRepository.GetById(id);
        }


        public Competition Add(Competition input)
        {
            return _competitionRepository.Create(input);
        }

        public Competition Update(int id, Competition input)
        {
            return _competitionRepository.Update(new Competition(id, input));
        }
    }
}
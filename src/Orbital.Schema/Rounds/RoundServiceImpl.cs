using System.Collections.Generic;
using System.Linq;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Schema.Rounds
{
    public class RoundServiceImpl : IRoundService
    {
        private readonly IRoundRepository _roundRepository;

        public RoundServiceImpl(IRoundRepository roundRepository)
        {
            _roundRepository = roundRepository;
        }

        public IReadOnlyCollection<Round> GetRoot()
        {
            return _roundRepository.GetAll();
        }

        public IReadOnlyCollection<Round> GetVariants(int parentId)
        {
            return _roundRepository.GetAllVariantsById(parentId);
        }

        public IReadOnlyCollection<Round> GetByCompetition(Competition competition)
        {
            return competition.Rounds.Select(roundId => _roundRepository.GetById(roundId)).ToList();
        }

        public Round GetById(int id)
        {
            return _roundRepository.GetById(id);
        }


        public Round Add(Round input)
        {
            return _roundRepository.Create(input);
        }

        public Round Update(int id, Round input)
        {
            return _roundRepository.Update(new Round(id, input));
        }
    }
}
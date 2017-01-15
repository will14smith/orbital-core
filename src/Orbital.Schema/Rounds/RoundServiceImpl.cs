using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }
        public Round GetById(int id)
        {
            throw new NotImplementedException();
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
using System;
using System.Collections.Generic;
using Orbital.Models.Domain;

namespace Orbital.Web.Rounds
{
    class RoundService : IRoundService
    {
        public IReadOnlyCollection<Round> GetAll()
        {
            throw new NotImplementedException();
        }

        public RoundViewModel GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Guid Create(RoundInputModel input)
        {
            throw new NotImplementedException();
        }

        public void Update(Guid id, RoundInputModel input)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
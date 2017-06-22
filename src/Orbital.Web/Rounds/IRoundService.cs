using System;
using System.Collections.Generic;
using Orbital.Models.Domain;

namespace Orbital.Web.Rounds
{
    public interface IRoundService
    {
        IReadOnlyCollection<Round> GetAll();

        RoundViewModel GetById(Guid id);

        Guid Create(RoundInputModel input);
        void Update(Guid id, RoundInputModel input);
        void Delete(Guid id);
    }
}
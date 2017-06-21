using System;
using System.Collections.Generic;
using Orbital.Models.Domain;

namespace Orbital.Web.Clubs
{
    public interface IClubService
    {
        IReadOnlyCollection<Club> GetAll();
        ClubViewModel GetById(Guid id);

        Guid Create(ClubInputModel club);
        void Update(Guid id, ClubInputModel club);
        void Delete(Guid id);
    }
}
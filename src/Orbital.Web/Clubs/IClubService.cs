using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orbital.Models.Domain;

namespace Orbital.Web.Clubs
{
    public interface IClubService
    {
        Task<IReadOnlyCollection<Club>> GetAll();
        Task<ClubViewModel> GetById(Guid id);

        Task<Guid> Create(ClubInputModel club);
        Task Update(Guid id, ClubInputModel club);
        Task Delete(Guid id);
    }
}
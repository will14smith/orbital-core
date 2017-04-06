using System.Collections.Generic;
using Orbital.Web.Models;

namespace Orbital.Web.Services
{
    public interface IClubService
    {
        IReadOnlyCollection<ClubViewModel> GetAll();
        ClubViewModel GetById(int id);

        ClubViewModel Create(ClubInputModel club);
        ClubViewModel Update(ClubInputModel club);
        bool Delete(int id);

    }
}
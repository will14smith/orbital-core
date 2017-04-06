using System.Collections.Generic;

namespace Orbital.Web.Clubs
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
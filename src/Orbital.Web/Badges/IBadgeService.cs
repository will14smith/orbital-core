using System.Collections.Generic;

namespace Orbital.Web.Badges
{
    public interface IBadgeService
    {
        IReadOnlyCollection<BadgeViewModel> GetAll();
        BadgeViewModel GetById(int id);

        BadgeViewModel Create(BadgeInputModel club);
        BadgeViewModel Update(BadgeInputModel club);
        bool Delete(int id);
    }
}

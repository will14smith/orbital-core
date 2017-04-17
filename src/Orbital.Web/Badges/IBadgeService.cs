using System.Collections.Generic;

namespace Orbital.Web.Badges
{
    public interface IBadgeService
    {
        IReadOnlyCollection<BadgeViewModel> GetAll();
        BadgeViewModel GetById(int id);

        BadgeViewModel Create(BadgeInputModel input);
        BadgeViewModel Update(BadgeInputModel input);
        bool Delete(int id);
    }
}

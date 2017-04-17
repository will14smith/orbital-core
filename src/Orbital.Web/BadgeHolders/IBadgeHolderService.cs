using System.Collections.Generic;

namespace Orbital.Web.BadgeHolders
{
    public interface IBadgeHolderService
    {
        IReadOnlyCollection<BadgeHolderViewModel> GetAllByBadgeId(int badgeId);
        IReadOnlyCollection<BadgeHolderViewModel> GetAllByPersonId(int personId);

        BadgeHolderViewModel GetById(int id);

        BadgeHolderViewModel Create(BadgeHolderInputModel input);
        BadgeHolderViewModel Update(BadgeHolderInputModel input);
        bool Delete(int id);
    }
}

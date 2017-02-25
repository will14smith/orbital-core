using System.Collections.Generic;
using Orbital.Models.Domain;

namespace Orbital.Models.Repositories
{
    public interface IBadgeHolderRepository : IRepository<BadgeHolder>
    {
        IReadOnlyCollection<BadgeHolder> GetAllByBadgeId(int badgeId);
        IReadOnlyCollection<BadgeHolder> GetAllByPersonId(int personId);
    }
}
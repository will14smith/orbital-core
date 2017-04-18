using System.Linq;
using Halcyon.HAL;
using Halcyon.Web.HAL;
using Microsoft.AspNetCore.Mvc;
using Orbital.Web.BadgeHolders;
using Orbital.Web.Helpers;

namespace Orbital.Web.Badges.BadgeHolders
{
    public class BadgeBadgeHolderController : Controller
    {
        private readonly IBadgeHolderService _badgeHolderService;

        public BadgeBadgeHolderController(IBadgeHolderService badgeHolderService)
        {
            _badgeHolderService = badgeHolderService;
        }

        [HttpGet("api/badge/{badgeId}/holders")]
        public IActionResult Get(int badgeId)
        {
            var holders = _badgeHolderService.GetAllByBadgeId(badgeId);

            return this.Paginate(holders.Select(ViewModelToResponse).ToList(), "badgeHolders");
        }

        private HALResponse ViewModelToResponse(BadgeHolderViewModel badgeHolder)
        {
            return new HALResponse(badgeHolder)
                .AddLinks(new Link("self", Url.Action("Get", "BadgeHolder", new { id = badgeHolder.Id })));
        }
    }
}

using System.Linq;
using Halcyon.HAL;
using Microsoft.AspNetCore.Mvc;
using Orbital.Web.BadgeHolders;
using Orbital.Web.Helpers;

namespace Orbital.Web.People.BadgeHolders
{
    public class PersonBadgeHolderController : Controller
    {
        private readonly IBadgeHolderService _badgeHolderService;

        public PersonBadgeHolderController(IBadgeHolderService badgeHolderService)
        {
            _badgeHolderService = badgeHolderService;
        }

        [HttpGet("api/person/{personId}/badges")]
        public IActionResult Get(int personId)
        {
            var holders = _badgeHolderService.GetAllByPersonId(personId);

            return this.Paginate(holders.Select(ViewModelToResponse).ToList(), "badgeHolders");
        }

        private HALResponse ViewModelToResponse(BadgeHolderViewModel badgeHolder)
        {
            return new HALResponse(badgeHolder)
                .AddLinks(new Link("self", Url.Action("Get", "BadgeHolder", new { id = badgeHolder.Id })));
        }
    }
}

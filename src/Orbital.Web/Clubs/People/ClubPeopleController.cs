using System.Linq;
using Halcyon.HAL;
using Microsoft.AspNetCore.Mvc;
using Orbital.Web.Helpers;
using Orbital.Web.People;

namespace Orbital.Web.Clubs.People
{
    public class ClubPeopleController : Controller
    {
        private readonly IPersonService _personService;

        public ClubPeopleController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet("api/club/{clubId}/people")]
        public IActionResult Get(int clubId)
        {
            var holders = _personService.GetAllByClubId(clubId);

            return this.Paginate(holders.Select(ViewModelToResponse).ToList(), "people");
        }

        private HALResponse ViewModelToResponse(PersonViewModel person)
        {
            return new HALResponse(person)
                .AddLinks(new Link("self", Url.Action("Get", "Person", new { id = person.Id })));
        }
    }
}

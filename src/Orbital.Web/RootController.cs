using Halcyon.HAL;
using Microsoft.AspNetCore.Mvc;

namespace Orbital.Web
{
    [Route("api")]
    public class RootController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new HALResponse(null).AddLinks(new[]
            {
                new Link("clubs", Url.Action("Get", "Club")),
                new Link("club", Url.Action("Get", "Club", new { id="{id}" })),
            }));
        }
    }
}

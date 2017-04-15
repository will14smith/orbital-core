using Halcyon.HAL;
using Halcyon.Web.HAL;
using Microsoft.AspNetCore.Mvc;
using Orbital.Web.Helpers;

namespace Orbital.Web.Badges
{
    [Route("api/[controller]")]
    public class BadgeController : Controller
    {
        private readonly IBadgeService _badgeService;

        public BadgeController(IBadgeService badgeService)
        {
            _badgeService = badgeService;
        }

        // GET: api/badge
        [HttpGet]
        public IActionResult Get()
        {
            var badges = _badgeService.GetAll();

            return this.Paginate(badges, "badges");
        }

        // GET api/badge/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var badge = _badgeService.GetById(id);
            if (badge == null)
            {
                return NotFound();
            }

            var response = ViewModelToResponse(badge);

            return Ok(response);
        }

        // POST api/badge
        [HttpPost]
        public IActionResult Post([FromBody]BadgeInputModel input)
        {
            if (input == null)
            {
                return BadRequest();
            }

            var item = _badgeService.Create(input);
            var response = ViewModelToResponse(item);

            return CreatedAtAction(nameof(Get), new { id = item.Id }, response);
        }

        // PUT api/badge/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]BadgeInputModel input)
        {
            if (input == null)
            {
                return BadRequest();
            }

            input.Id = id;

            var item = _badgeService.Update(input);
            var response = ViewModelToResponse(item);

            return AcceptedAtAction(nameof(Get), new { id = item.Id }, response);
        }

        // DELETE api/badge/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var badge = _badgeService.GetById(id);
            if (badge == null)
            {
                return NotFound();
            }

            _badgeService.Delete(id);
            return Accepted();
        }

        private HALResponse ViewModelToResponse(BadgeViewModel badge)
        {
            return new HALResponse(badge)
                .AddSelfLink(Request);
        }
    }
}

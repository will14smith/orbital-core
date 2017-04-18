using Halcyon.HAL;
using Halcyon.Web.HAL;
using Microsoft.AspNetCore.Mvc;

namespace Orbital.Web.BadgeHolders
{
    [Route("api/[controller]")]
    public class BadgeHolderController : Controller
    {
        private readonly IBadgeHolderService _badgeHolderService;

        public BadgeHolderController(IBadgeHolderService badgeHolderService)
        {
            _badgeHolderService = badgeHolderService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var badgeHolder = _badgeHolderService.GetById(id);
            if (badgeHolder == null)
            {
                return NotFound();
            }

            var response = ViewModelToResponse(badgeHolder);

            return Ok(response);
        }

        // POST api/badgeHolder
        [HttpPost]
        public IActionResult Post([FromBody]BadgeHolderInputModel input)
        {
            if (input == null)
            {
                return BadRequest();
            }

            var item = _badgeHolderService.Create(input);
            var response = ViewModelToResponse(item);

            return CreatedAtAction(nameof(Get), new { id = item.Id }, response);
        }

        // PUT api/badgeHolder/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]BadgeHolderInputModel input)
        {
            if (input == null)
            {
                return BadRequest();
            }

            input.Id = id;

            var item = _badgeHolderService.Update(input);
            var response = ViewModelToResponse(item);

            return AcceptedAtAction(nameof(Get), new { id = item.Id }, response);
        }

        // DELETE api/badgeHolder/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var badgeHolder = _badgeHolderService.GetById(id);
            if (badgeHolder == null)
            {
                return NotFound();
            }

            _badgeHolderService.Delete(id);
            return Accepted();
        }

        private HALResponse ViewModelToResponse(BadgeHolderViewModel badgeHolder)
        {
            return new HALResponse(badgeHolder)
                .AddLinks(new Link("self", Url.Action("Get", new { id = badgeHolder.Id })));
        }
    }
}

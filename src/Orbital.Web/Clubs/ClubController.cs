using System.Linq;
using Halcyon.HAL;
using Halcyon.Web.HAL;
using Microsoft.AspNetCore.Mvc;
using Orbital.Web.Helpers;

namespace Orbital.Web.Clubs
{
    [Route("api/[controller]")]
    public class ClubController : Controller
    {
        private readonly IClubService _clubService;

        public ClubController(IClubService clubService)
        {
            _clubService = clubService;
        }

        // GET: api/club
        [HttpGet]
        public IActionResult Get()
        {
            var clubs = _clubService.GetAll();

            return this.Paginate(clubs.Select(ViewModelToResponse).ToList(), "clubs");
        }

        // GET api/club/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var club = _clubService.GetById(id);
            if (club == null)
            {
                return NotFound();
            }

            var response = ViewModelToResponse(club);

            return Ok(response);
        }

        // POST api/club
        [HttpPost]
        public IActionResult Post([FromBody]ClubInputModel input)
        {
            if (input == null)
            {
                return BadRequest();
            }

            var item = _clubService.Create(input);
            var response = ViewModelToResponse(item);

            return CreatedAtAction(nameof(Get), new { id = item.Id }, response);
        }

        // PUT api/club/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]ClubInputModel input)
        {
            if (input == null)
            {
                return BadRequest();
            }

            input.Id = id;

            var item = _clubService.Update(input);
            var response = ViewModelToResponse(item);

            return AcceptedAtAction(nameof(Get), new { id = item.Id }, response);
        }

        // DELETE api/club/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var club = _clubService.GetById(id);
            if (club == null)
            {
                return NotFound();
            }

            _clubService.Delete(id);
            return Accepted();
        }

        private HALResponse ViewModelToResponse(ClubViewModel club)
        {
            return new HALResponse(club)
                .AddLinks(new Link("self", Url.Action("Get", new { id = club.Id })));
        }
    }
}

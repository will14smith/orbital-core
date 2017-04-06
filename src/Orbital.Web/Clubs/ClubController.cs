using System;
using System.Collections.Generic;
using Halcyon.HAL;
using Halcyon.Web.HAL;
using Microsoft.AspNetCore.Mvc;

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

            var page = 1;
            var pageSize = 100000;
            var total = clubs.Count;

            var navLinks = BuildNavLinks(page, pageSize, total);

            var response = new HALResponse(new { count = clubs.Count, total = total })
                .AddSelfLink(Request)
                .AddLinks(navLinks)
                .AddEmbeddedCollection("clubs", clubs);

            return Ok(response);
        }

        private IEnumerable<Link> BuildNavLinks(int page, int pageSize, int totalItemCount)
        {
            var totalPages = (int)Math.Ceiling((decimal)totalItemCount / pageSize);

            yield return new Link("first", Url.Action("Get", new { page = 1 }));
            if (page > 1)
            {
                yield return new Link("prev", Url.Action("Get", new { page = page - 1 }));
            }
            if (page < totalPages)
            {
                yield return new Link("next", Url.Action("Get", new { page = page + 1 }));
            }
            yield return new Link("last", Url.Action("Get", new { page = totalPages }));
        }

        // GET api/club/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var club = _clubService.GetById(id);

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
                .AddSelfLink(Request);
        }
    }
}

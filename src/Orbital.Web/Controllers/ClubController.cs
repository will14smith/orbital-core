using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Orbital.Web.Models;
using Orbital.Web.Services;

namespace Orbital.Web.Controllers
{
    [Route("api/[controller]")]
    public class ClubController : Controller
    {
        private readonly IClubService _clubService;

        public ClubController(IClubService clubService)
        {
            _clubService = clubService;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<ClubViewModel> Get()
        {
            return _clubService.GetAll();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ClubViewModel Get(int id)
        {
            return _clubService.GetById(id);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]ClubInputModel input)
        {
            if (input == null)
            {
                return BadRequest();
            }

            var item = _clubService.Create(input);

            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]ClubInputModel input)
        {
            if (input == null)
            {
                return BadRequest();
            }

            input.Id = id;

            _clubService.Update(input);
            return NoContent();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var club = _clubService.GetById(id);
            if (club == null)
            {
                return NotFound();
            }

            _clubService.Delete(id);
            return new NoContentResult();

        }
    }
}

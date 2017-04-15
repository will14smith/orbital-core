using System.Collections.Generic;
using Halcyon.HAL;
using Halcyon.Web.HAL;
using Microsoft.AspNetCore.Mvc;
using Orbital.Web.Helpers;

namespace Orbital.Web.Rounds
{
    [Route("api/[controller]")]
    public class RoundController : Controller
    {
        private readonly IRoundService _roundService;

        public RoundController(IRoundService roundService)
        {
            _roundService = roundService;
        }

        // GET: api/round
        [HttpGet]
        public IActionResult Get()
        {
            var rounds = _roundService.GetAll();

            return this.Paginate(rounds, "rounds");
        }

        // GET api/round/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var round = _roundService.GetById(id);
            if (round == null)
            {
                return NotFound();
            }

            var response = ViewModelToResponse(round);

            return Ok(response);
        }

        // GET api/round/5/variants
        [HttpGet("{id}/variants")]
        public IActionResult GetVariants(int id)
        {
            var rounds = _roundService.GetAllByVariant(id);

            return this.Paginate(rounds, "rounds");
        }

        // POST api/round
        [HttpPost]
        public IActionResult Post([FromBody]RoundInputModel input)
        {
            if (input == null)
            {
                return BadRequest();
            }

            var item = _roundService.Create(input);
            var response = ViewModelToResponse(item);

            return CreatedAtAction(nameof(Get), new { id = item.Id }, response);
        }

        // PUT api/round/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]RoundInputModel input)
        {
            if (input == null)
            {
                return BadRequest();
            }

            input.Id = id;

            var item = _roundService.Update(input);
            var response = ViewModelToResponse(item);

            return AcceptedAtAction(nameof(Get), new { id = item.Id }, response);
        }

        // DELETE api/round/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var round = _roundService.GetById(id);
            if (round == null)
            {
                return NotFound();
            }

            _roundService.Delete(id);
            return Accepted();
        }

        private HALResponse ViewModelToResponse(RoundViewModel round)
        {
            var links = new List<Link>
            {
                new Link("variants", Url.Action("GetVariants", new {id = round.Id}))
            };

            if (round.VariantOfId.HasValue)
            {
                links.Add(new Link("parent", Url.Action("Get", new { id = round.VariantOfId.Value })));
            }

            return new HALResponse(round)
                .AddSelfLink(Request)
                .AddLinks(links);
        }
    }
}

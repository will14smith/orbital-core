using System;
using System.Collections.Generic;
using Halcyon.HAL;
using Halcyon.Web.HAL;
using Microsoft.AspNetCore.Mvc;

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

            var page = 1;
            var pageSize = 100000;
            var total = rounds.Count;

            var navLinks = BuildNavLinks(page, pageSize, total);

            var response = new HALResponse(new { count = rounds.Count, total = total })
                .AddSelfLink(Request)
                .AddLinks(navLinks)
                .AddEmbeddedCollection("rounds", rounds);

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

            var page = 1;
            var pageSize = 100000;
            var total = rounds.Count;

            var navLinks = BuildNavLinks(page, pageSize, total);

            var response = new HALResponse(new { count = rounds.Count, total = total })
                .AddSelfLink(Request)
                .AddLinks(new Link("parent", Url.Action("Get", new { id })))
                .AddLinks(navLinks)
                .AddEmbeddedCollection("rounds", rounds);

            return Ok(response);
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

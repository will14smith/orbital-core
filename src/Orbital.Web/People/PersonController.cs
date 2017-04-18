using System.Linq;
using Halcyon.HAL;
using Halcyon.Web.HAL;
using Microsoft.AspNetCore.Mvc;
using Orbital.Web.Helpers;

namespace Orbital.Web.People
{
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        // GET: api/person
        [HttpGet]
        public IActionResult Get()
        {
            var people = _personService.GetAll();

            return this.Paginate(people.Select(ViewModelToResponse).ToList(), "people");
        }

        // TODO GET api/club/5/people

        // GET api/person/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var person = _personService.GetById(id);
            if (person == null)
            {
                return NotFound();
            }

            var response = ViewModelToResponse(person);

            return Ok(response);
        }

        // POST api/person
        [HttpPost]
        public IActionResult Post([FromBody]PersonInputModel input)
        {
            if (input == null)
            {
                return BadRequest();
            }

            var item = _personService.Create(input);
            var response = ViewModelToResponse(item);

            return CreatedAtAction(nameof(Get), new { id = item.Id }, response);
        }

        // PUT api/person/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]PersonInputModel input)
        {
            if (input == null)
            {
                return BadRequest();
            }

            input.Id = id;

            var item = _personService.Update(input);
            var response = ViewModelToResponse(item);

            return AcceptedAtAction(nameof(Get), new { id = item.Id }, response);
        }

        // DELETE api/person/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var person = _personService.GetById(id);
            if (person == null)
            {
                return NotFound();
            }

            _personService.Delete(id);
            return Accepted();
        }

        private HALResponse ViewModelToResponse(PersonViewModel person)
        {
            return new HALResponse(person)
                .AddLinks(new Link("self", Url.Action("Get", new { id = person.Id })));
        }
    }
}

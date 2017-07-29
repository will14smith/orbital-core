using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Orbital.Web.Clubs;

namespace Orbital.Web.People
{
    [Route("people")]
    public class PersonController : Controller
    {
        private readonly IPersonService _personService;
        private readonly IClubService _clubService;

        public PersonController(IPersonService personService, IClubService clubService)
        {
            _personService = personService;
            _clubService = clubService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var people = await _personService.GetAll();

            return View(people);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var person = await _personService.GetById(id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var input = new PersonInputModel();
             
            ViewBag.Clubs = await GetClubsForSelectList();
            return View(input);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create(PersonInputModel input)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Clubs = await GetClubsForSelectList();
                return View(input);
            }

            var id = await _personService.Create(input);

            return RedirectToAction(nameof(Get), new { id = id });
        }

        [HttpGet("{id}/edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var person = await _personService.GetById(id);
            if (person == null)
            {
                return NotFound();
            }

            var input = new PersonInputModel(person.Person);

            ViewBag.Clubs = await GetClubsForSelectList();
            return View(input);
        }
        [HttpPost("{id}/edit")]
        public async Task<IActionResult> Edit(Guid id, PersonInputModel input)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Clubs = await GetClubsForSelectList();
                return View(input);
            }

            await _personService.Update(id, input);

            return RedirectToAction(nameof(Get), new { id });
        }

        [HttpPost("{id}/delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var person = _personService.GetById(id);
            if (person == null)
            {
                return NotFound();
            }

            await _personService.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetClubsForSelectList()
        {
            var clubs = await _clubService.GetAll();

            return clubs.Select(x => new SelectListItem {Value = x.Id.ToString(), Text = x.Name});
        }
    }
}

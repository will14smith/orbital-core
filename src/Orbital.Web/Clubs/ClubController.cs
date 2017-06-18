using System;
using Microsoft.AspNetCore.Mvc;

namespace Orbital.Web.Clubs
{
    [Route("clubs")]
    public class ClubController : Controller
    {
        private readonly IClubService _clubService;

        public ClubController(IClubService clubService)
        {
            _clubService = clubService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var clubs = _clubService.GetAll();

            return View(clubs);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var club = _clubService.GetById(id);
            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            var input = new ClubInputModel();

            return View(input);
        }
        [HttpPost("create")]
        public IActionResult Create(ClubInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var id = _clubService.Create(input);

            return RedirectToAction(nameof(Get), new { id = id });
        }

        [HttpGet("{id}/edit")]
        public IActionResult Edit(Guid id)
        {
            var club = _clubService.GetById(id);
            if (club == null)
            {
                return NotFound();
            }

            var input = new ClubInputModel(club.Club);

            return View(input);
        }
        [HttpPost("{id}/edit")]
        public IActionResult Edit(Guid id, ClubInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            _clubService.Update(id, input);

            return RedirectToAction(nameof(Get), new { id });
        }

        [HttpPost("{id}/delete")]
        public IActionResult Delete(Guid id)
        {
            var club = _clubService.GetById(id);
            if (club == null)
            {
                return NotFound();
            }

            _clubService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}

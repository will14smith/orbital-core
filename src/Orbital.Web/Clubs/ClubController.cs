using System;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Index()
        {
            var clubs = await _clubService.GetAll();

            return View(clubs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var club = await _clubService.GetById(id);
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
        public async Task<IActionResult> Create(ClubInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var id = await _clubService.Create(input);

            return RedirectToAction(nameof(Get), new { id = id });
        }

        [HttpGet("{id}/edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var club = await _clubService.GetById(id);
            if (club == null)
            {
                return NotFound();
            }

            var input = new ClubInputModel(club.Club);

            return View(input);
        }
        [HttpPost("{id}/edit")]
        public async Task<IActionResult> Edit(Guid id, ClubInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            await _clubService.Update(id, input);

            return RedirectToAction(nameof(Get), new { id });
        }

        [HttpPost("{id}/delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var club = _clubService.GetById(id);
            if (club == null)
            {
                return NotFound();
            }

            await _clubService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}

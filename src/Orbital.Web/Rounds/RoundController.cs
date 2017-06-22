using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Orbital.Web.Rounds
{
    [Route("rounds")]
    public class RoundController : Controller
    {
        private readonly IRoundService _roundService;

        public RoundController(IRoundService roundService)
        {
            _roundService = roundService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var rounds = _roundService.GetAll();

            return View(rounds);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var round = _roundService.GetById(id);
            if (round == null)
            {
                return NotFound();
            }

            return View(round);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            var input = new RoundInputModel();

            ViewData["Rounds"] = _roundService.GetAll().Select(x => new RoundSummaryModel(x)).ToList();

            return View(input);
        }
        [HttpPost("create")]
        public IActionResult Create(RoundInputModel input)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Rounds"] = _roundService.GetAll().Select(x => new RoundSummaryModel(x)).ToList();

                return View(input);
            }

            var id = _roundService.Create(input);

            return RedirectToAction(nameof(Get), new { id = id });
        }

        [HttpGet("{id}/edit")]
        public IActionResult Edit(Guid id)
        {
            var round = _roundService.GetById(id);
            if (round == null)
            {
                return NotFound();
            }

            var input = new RoundInputModel(round.Round);

            ViewData["Rounds"] = _roundService.GetAll().Where(x => x.Id != id).Select(x => new RoundSummaryModel(x)).ToList();

            return View(input);
        }
        [HttpPost("{id}/edit")]
        public IActionResult Edit(Guid id, RoundInputModel input)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Rounds"] = _roundService.GetAll().Where(x => x.Id != id).Select(x => new RoundSummaryModel(x)).ToList();

                return View(input);
            }

            _roundService.Update(id, input);

            return RedirectToAction(nameof(Get), new { id });
        }

        [HttpPost("{id}/delete")]
        public IActionResult Delete(Guid id)
        {
            var round = _roundService.GetById(id);
            if (round == null)
            {
                return NotFound();
            }

            _roundService.Delete(id);

            return RedirectToAction(nameof(Index));

        }
    }
}

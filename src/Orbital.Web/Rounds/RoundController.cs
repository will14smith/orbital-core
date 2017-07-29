using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Index()
        {
            var rounds = await _roundService.GetAll();

            return View(rounds);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var round = await _roundService.GetById(id);
            if (round == null)
            {
                return NotFound();
            }

            return View(round);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var input = new RoundInputModel();

            var rounds = await _roundService.GetAll();
            ViewData["Rounds"] = rounds.Select(x => new RoundSummaryModel(x)).ToList();

            return View(input);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] RoundInputModel input)
        {
            if (!ModelState.IsValid)
            {
                var rounds = await _roundService.GetAll();
                ViewData["Rounds"] = rounds.Select(x => new RoundSummaryModel(x)).ToList();

                return View(input);
            }

            var id = await _roundService.Create(input);

            return RedirectToGet(id);
        }

        [HttpGet("{id}/edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var round = await _roundService.GetById(id);
            if (round == null)
            {
                return NotFound();
            }

            var input = new RoundInputModel(round.Round);

            var rounds = await _roundService.GetAll();
            ViewData["Rounds"] = rounds.Where(x => x.Id != id).Select(x => new RoundSummaryModel(x)).ToList();

            return View(input);
        }

        [HttpPost("{id}/edit")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] RoundInputModel input)
        {
            if (!ModelState.IsValid)
            {
                var rounds = await _roundService.GetAll();
                ViewData["Rounds"] = rounds.Where(x => x.Id != id).Select(x => new RoundSummaryModel(x)).ToList();

                return View(input);
            }

            await _roundService.Update(id, input);

            return RedirectToGet(id);
        }

        [HttpPost("{id}/delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var round = _roundService.GetById(id);
            if (round == null)
            {
                return NotFound();
            }

            await _roundService.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        private IActionResult RedirectToGet(Guid id)
        {
            var response = Json(new {url = Url.Action(nameof(Get), new {id = id})});
            response.StatusCode = (int) HttpStatusCode.Accepted;

            return response;
        }
    }
}

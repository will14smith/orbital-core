using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Orbital.Web.Scores
{
    [Route("scores")]
    public class ScoreController : Controller
    {
        private readonly IScoreService _scoreService;

        public ScoreController(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var scores = await _scoreService.GetAll();

            return View(scores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var score = await _scoreService.GetById(id);
            if (score == null)
            {
                return NotFound();
            }

            return View(score);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var input = new ScoreInputModel();

            await PopulateAssociatedFormData();
            return View(input);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ScoreInputModel input)
        {
            if (!ModelState.IsValid)
            {
                await PopulateAssociatedFormData();
                return View(input);
            }

            var id = await _scoreService.Create(input);

            return RedirectToGet(id);
        }

        [HttpGet("{id}/edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var score = await _scoreService.GetById(id);
            if (score == null)
            {
                return NotFound();
            }

            var input = new ScoreInputModel(score.Score);

            await PopulateAssociatedFormData();
            return View(input);
        }

        [HttpPost("{id}/edit")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] ScoreInputModel input)
        {
            if (!ModelState.IsValid)
            {
                await PopulateAssociatedFormData();
                return View(input);
            }

            await _scoreService.Update(id, input);

            return RedirectToGet(id);
        }

        [HttpPost("{id}/delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var round = _scoreService.GetById(id);
            if (round == null)
            {
                return NotFound();
            }

            await _scoreService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
        
        private async Task PopulateAssociatedFormData()
        {
            ViewData["people"] = null;
            ViewData["clubs"] = null;
            ViewData["bowstyles"] = null;
            ViewData["rounds"] = null;
            ViewData["competitions"] = null;
        }

        private IActionResult RedirectToGet(Guid id)
        {
            var response = Json(new { url = Url.Action(nameof(Get), new { id = id }) });
            response.StatusCode = (int)HttpStatusCode.Accepted;

            return response;
        }
    }
}

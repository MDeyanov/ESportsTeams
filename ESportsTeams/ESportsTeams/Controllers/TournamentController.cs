 using ESportsTeams.Core.Interfaces;
using ESportsTeams.Infrastructure.Data.Entity;
using Microsoft.AspNetCore.Mvc;

namespace ESportsTeams.Controllers
{
    public class TournamentController : Controller
    {
        private readonly ITournamentService _tournamentService;

        public TournamentController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        [HttpGet]
        public async Task<IActionResult> Game(int id)
        {
            var model = await _tournamentService.GetTournamentByEventIdAsync(id);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var tournament = await _tournamentService.GetTournamentDetailsByIdAsync(id);
            return tournament == null ? NotFound() : View(tournament);
        }

        //[HttpPost]
        //public async Task<IActionResult> Join()
        //{

        //}
    }
}

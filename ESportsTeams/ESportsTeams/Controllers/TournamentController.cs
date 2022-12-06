 using ESportsTeams.Core.Interfaces;
using ESportsTeams.Infrastructure.Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ESportsTeams.Controllers
{
    [Authorize]
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

        public async Task<IActionResult> Join(int Id)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                await _tournamentService.TeamJoinToTournaments(userId, Id);
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction(nameof(TeamsList));
        }

        [HttpGet]
        public async Task<IActionResult> TeamsList(int id)
        {
            var teams = await _tournamentService.ListOfTeamsInTournamentAsync(id);
            return View(teams);
        }
    }
}

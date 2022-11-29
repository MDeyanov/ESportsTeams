using ESportsTeams.Core.Interfaces;
using ESportsTeams.Core.Models.BindingModels.Tournament;
using ESportsTeams.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace ESportsTeams.Areas.Administrator.Controllers
{
    public class TournamentController : AdminController
    {
        public readonly ITournamentService _tournamentService;

        public TournamentController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Add()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTournamentBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {

                await _tournamentService.AddTournamentAsync(model);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {

                ModelState.AddModelError("", "Something went wrong!");

                return View(model);
            }
        }
    }
}

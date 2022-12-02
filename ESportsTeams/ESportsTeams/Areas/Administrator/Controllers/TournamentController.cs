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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var tournamentToEdit = await _tournamentService.GetTournamentByIdAsync(id);
            if (tournamentToEdit == null)
            {
                return View("Error");
            }
            var tournamentModel = new EditTournamentBindingModel()
            {
                Id = tournamentToEdit.Id,
                Title = tournamentToEdit.Title,
                Description = tournamentToEdit.Description,
                StartTime = tournamentToEdit.StartTime,
                EntryFee = tournamentToEdit.EntryFee,
                Facebook = tournamentToEdit.Facebook,
                Website = tournamentToEdit.Website,
                Contact = tournamentToEdit.Contact,
                Twitter = tournamentToEdit.Twitter,
                PrizePool = tournamentToEdit.PrizePool,
                Address = tournamentToEdit.Address,
                EventTitle = tournamentToEdit.Event.Title
            };
            return View(tournamentModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditTournamentBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit Tournament");
                return View("Edit", model);
            }

            await _tournamentService.EditTournamentAsync(model);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var tournament = await _tournamentService.GetTournamentDetailsByIdAsync(id);
            return tournament == null ? NotFound() : View(tournament);
        }
    }
}

using ESportsTeams.Core.Interfaces;
using ESportsTeams.Core.Models.BindingModels.Team;
using ESportsTeams.Infrastructure.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ESportsTeams.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly UserManager<AppUser> _userManager;

        public TeamController(UserManager<AppUser> userManager, ITeamService teamService)
        {
            _userManager = userManager;
           _teamService = teamService;
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
        public async Task<IActionResult> Add(AddTeamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var loggedUser = this.User;
                var dbUserId = _userManager.GetUserId(loggedUser);
                await _teamService.AddTeamAsync(model, dbUserId);
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

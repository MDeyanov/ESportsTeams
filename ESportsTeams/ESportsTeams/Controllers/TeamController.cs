using ESportsTeams.Core.Interfaces;
using ESportsTeams.Core.Models.BindingModels.Team;
using ESportsTeams.Core.Models.ViewModels.TeamViewModels;
using ESportsTeams.Core.Services;
using ESportsTeams.Infrastructure.Data.Entity;
using ESportsTeams.Infrastructure.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Linq;

namespace ESportsTeams.Controllers
{
    [Authorize]
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly IPhotoService _photoService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;

        public TeamController(
            UserManager<AppUser> userManager,
            ITeamService teamService,
            IPhotoService photoService,
            IUserService userService)
        {
            _userManager = userManager;
            _teamService = teamService;
            _photoService = photoService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int category = -1, int page = 1, int pageSize = 6)
        {
            if (page < 1 || pageSize < 1)
            {
                return NotFound();
            }

            var teams = category switch
            {
                -1 => await _teamService.GetSliceAsync((page - 1) * pageSize, pageSize),
                _ => await _teamService.GetTeamsByCategoryAndSliceAsync((Category)category, (page - 1) * pageSize, pageSize),
            };

            var count = category switch
            {
                -1 => await _teamService.GetCountAsync(),
                _ => await _teamService.GetCountByCategoryAsync((Category)category),
            };
            var loggedUser = this.User;
            var dbUserId = _userManager.GetUserId(loggedUser);

            var teamViewModel = new IndexTeamViewModel
            {
                loggedUserId = dbUserId,
                Teams = teams,
                Page = page,
                PageSize = pageSize,
                TotalTeams = count,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                Category = category,
            };

            return View(teamViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> OwnedTeams(int category = -1, int page = 1, int pageSize = 6)
        {
            if (page < 1 || pageSize < 1)
            {
                return NotFound();
            }
            var loggedUser = this.User;
            var dbUserId = _userManager.GetUserId(loggedUser);

            var teams = category switch
            {
                -1 => await _teamService.GetSliceOfUserOwnedAsync(dbUserId, (page - 1) * pageSize, pageSize),
                _ => await _teamService.GetOwnedTeams(dbUserId, (Category)category, (page - 1) * pageSize, pageSize),
            };

            var count = category switch
            {
                -1 => await _teamService.GetOwnedTeamCountAsync(dbUserId),
                _ => await _teamService.GetCountByCategoryOfUserOwnedAsync(dbUserId, (Category)category),
            };

            var teamViewModel = new OwnTeamsViewModel
            {
                Teams = teams,
                Page = page,
                PageSize = pageSize,
                TotalTeams = count,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                Category = category,
            };

            return View(teamViewModel);
        }
        [HttpGet]
        public IActionResult Add()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTeamBindingModel model)
        {
            var teamExists = _teamService.TeamExistsAsync(model.Name);
            
            if (teamExists.Result)
            {
                ModelState.AddModelError("Name", "Name is already exist");
                return View(model);
            }
            var loggedUser = this.User;
            var dbUserId = _userManager.GetUserId(loggedUser);
            var currentUserTeamsCategories = _userService.CurrentUserTeamsHaveCategory(dbUserId, model.Category);

            if (!currentUserTeamsCategories.Result)
            {
                ModelState.AddModelError("Category", "You already have team with same category");
                return View(model);
            }
            
            // da napravq proverka dali toqzi user e owner na otbor ot syshtata kategoriq i ako e da ne mu pozvolq da napravi syshtiq team
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {              
                await _teamService.AddTeamAsync(model, dbUserId);
                return RedirectToAction(nameof(OwnedTeams)); 
            }
            catch (Exception)
            {

                ModelState.AddModelError("Image", "Something went wrong!");

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var loggedUserId = _userManager.GetUserId(this.User);
            var team = await _teamService.GetTeamDetailsByIdAsync(id, loggedUserId);

            return team == null ? NotFound() : View(team);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null)
            {
                return View("Error");
            }
            var teamViewModel = new EditTeamBindingModel()
            {
                Name = team.Name,
                Description = team.Description,
                Address = team.Address,
                URL = team.Image,
                Category = team.Category,
            };

            return View(teamViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTeamBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit Team");
                return View("Edit", model);
            }

            await _teamService.EditTeamAsync(model);
            return RedirectToAction("Index", "Home");
        }    
    }
}

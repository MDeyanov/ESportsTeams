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

namespace ESportsTeams.Controllers
{
    [Authorize]
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly IPhotoService _photoService;
        private readonly UserManager<AppUser> _userManager;

        public TeamController(
            UserManager<AppUser> userManager,
            ITeamService teamService,
            IPhotoService photoService)
        {
            _userManager = userManager;
            _teamService = teamService;
            _photoService = photoService;
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
        public async Task<IActionResult> Add(AddTeamViewModel model)
        {
            var teamExists = _teamService.TeamExistsAsync(model.Name);
            if (teamExists.Result)
            {
                ModelState.AddModelError("Name", "Name is already exist");
                return View(model);
            }
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
            var teamViewModel = new EditTeamViewModel()
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
        public async Task<IActionResult> Edit(EditTeamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit Team");
                return View("Edit", model);
            }

            await _teamService.EditTeamAsync(model);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var teamDetails = await _teamService.GetTeamByIdAsync(id);
            if (teamDetails == null)
            {
                return View("Error");
            }
            return View(teamDetails);
        }

        //[HttpPost, ActionName("Delete")]
        //public async Task<IActionResult> DeleteTeam(int id)
        //{
        //    var loggedUser = this.User;
        //    var dbUserId = _userManager.GetUserId(loggedUser);

        //    var team = await _teamService.GetTeamByIdAsync(id);
        //    if (team == null || team.OwnerId != dbUserId)
        //    {
        //        return View("Error");
        //    }
            

        //    var teamDelete = await _teamService.DeleteTeamAsync(id);


        //    if (!teamDelete)
        //    {
        //        return View("Error");
        //    }

        //    return RedirectToAction("Index");
        //}
    }


}

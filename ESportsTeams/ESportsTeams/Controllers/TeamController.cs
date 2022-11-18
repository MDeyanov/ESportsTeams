using ESportsTeams.Core.Interfaces;
using ESportsTeams.Core.Models.BindingModels.Team;
using ESportsTeams.Core.Models.ViewModels.TeamViewModels;
using ESportsTeams.Infrastructure.Data.Entity;
using ESportsTeams.Infrastructure.Data.Enums;
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

            var teamViewModel = new IndexTeamViewModel
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
                _ => await _teamService.GetOwnedTeams(dbUserId,(Category)category, (page - 1) * pageSize, pageSize),
            };

            var count = category switch
            {
                -1 => await _teamService.GetOwnedTeamCountAsync(dbUserId),
                _ => await _teamService.GetCountByCategoryOfUserOwnedAsync(dbUserId,(Category)category),
            };

            var teamViewModel = new IndexTeamViewModel
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
        [Route("Team/{esportsTeam}/{id}")]
        public async Task<IActionResult> DetailsTeam(int id)
        {
            var team = await _teamService.GetByIdAsync(id);

            return team == null ? NotFound() : View(team);
        }
    }

    
}

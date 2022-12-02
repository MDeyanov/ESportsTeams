using CloudinaryDotNet.Actions;
using ESportsTeams.Core.Interfaces;
using ESportsTeams.Infrastructure.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static ESportsTeams.Infrastructure.Data.Common.CommonConstants;

namespace ESportsTeams.Areas.Administrator.Controllers
{
    public class TeamController : AdminController
    {
        private readonly ITeamService _teamService;
        private readonly UserManager<AppUser> _userManager;

        public TeamController(ITeamService teamService, UserManager<AppUser> userManager)
        {
            _teamService = teamService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var loggedUserId = _userManager.GetUserId(this.User);
            var team = await _teamService.GetTeamDetailsByIdAsync(id, loggedUserId);

            return team == null ? NotFound() : View(team);
        }
        [HttpGet]
        public async Task<IActionResult> Index(string searchBy, string searchValue)
        {
            var teams = await _teamService.GetAllTeamsAsync();
            if (teams == null)
            {
                TempData[ErrorMessage] = NotFoundMessage;
            }
            else
            {
                if (string.IsNullOrEmpty(searchValue))
                {
                    TempData[WarningMessage] = EnterSearchValue;
                    return View(teams);
                }
                else
                {
                    if (searchBy.ToLower() == "teamname")
                    {
                        var searchByUsername = teams.Where(t => t.Name.ToLower() == searchValue.ToLower());
                        return View(searchByUsername);
                    }
                    else if (searchBy.ToLower() == "ownerusername")
                    {
                        var searchByOwnerName = teams.Where(t => t.Owner.UserName.ToLower() == searchValue.ToLower());
                        return View(searchByOwnerName);
                    }
                    else if (searchBy.ToLower() == "banned")
                    {
                        if (searchValue.ToLower() == "banned")
                        {
                            var searchBybanned = teams.Where(t => t.IsBanned == true);
                            return View(searchBybanned);
                        }
                    }
                    else if (searchBy.ToLower() == "notbanned")
                    {
                        if (searchValue.ToLower() == "notbanned")
                        {
                            var searchByNotBanned = teams.Where(t => t.IsBanned == false);
                            return View(searchByNotBanned);
                        }
                    }
                }
            }
            return View(teams);
        }
    }
}

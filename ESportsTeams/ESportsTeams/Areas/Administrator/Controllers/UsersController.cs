using CloudinaryDotNet;
using ESportsTeams.Core.Interfaces;
using ESportsTeams.Core.Services;
using ESportsTeams.Infrastructure.Data.Common;
using ESportsTeams.Infrastructure.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static ESportsTeams.Infrastructure.Data.Common.CommonConstants;

namespace ESportsTeams.Areas.Administrator.Controllers
{
    public class UsersController : AdminController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAdminService _adminService;

        public UsersController(IAdminService adminService, UserManager<AppUser> userManager)
        {
            _adminService = adminService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index(string searchBy, string searchValue)
        {
            var currentUserId = _userManager.GetUserAsync(User).Result.Id;

            var users = _adminService.GetAllUsers(currentUserId);
            
            if (users.Count == 0)
            {
                TempData[ErrorMessage] = UsersNotFoundMessage;
            }
            else
            {
                if (string.IsNullOrEmpty(searchValue))
                {
                    TempData[WarningMessage] = EnterSearchValue;
                    return View(users);
                }
                else
                {
                    if (searchBy.ToLower() == "username")
                    {
                        var searchByUsername = users.Where(x => x.Username.ToLower().Contains(searchValue.ToLower()));
                        return View(searchByUsername);
                    }
                    else if (searchBy.ToLower() == "id")
                    {
                        var searchById = users.Where(x=>x.Id.ToLower().Contains(searchValue.ToLower()));
                        return View(searchById);
                    }
                }
            }

            return View(users);
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            var user =  _adminService.GetUser(id);

            return user == null ? NotFound() : View(user);
        }

        [HttpGet]
        public IActionResult Ban(string Id)
        {
            string resultId = this._adminService.BanUser(Id);

            if (resultId == ErrorUserId)
            {
                TempData[ErrorMessage] = NotFoundMessage;
                return Index(string.Empty,string.Empty);
            }

            return Redirect(string.Format(RedirectConstants.AdministratorAreaUserDetailsPage, resultId));
        }

        [HttpGet]
        public IActionResult Unban(string Id) 
        {
            string resultId = _adminService.UnbanUser(Id);

            if (resultId == ErrorUserId)
            {
                TempData[ErrorMessage] = NotFoundMessage;
                return Index(string.Empty, string.Empty);
            }
            return Redirect(string.Format(RedirectConstants.AdministratorAreaUserDetailsPage, resultId));
        }
    }
}

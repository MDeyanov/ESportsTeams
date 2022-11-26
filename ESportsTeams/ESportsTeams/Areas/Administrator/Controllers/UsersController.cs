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
        public IActionResult Index()
        {
            var currentUserId = _userManager.GetUserAsync(User).Result.Id;

            var users = _adminService.GetAllUsers(currentUserId);

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
                return Index();
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
                return Index();
            }
            return Redirect(string.Format(RedirectConstants.AdministratorAreaUserDetailsPage, resultId));
        }
    }
}

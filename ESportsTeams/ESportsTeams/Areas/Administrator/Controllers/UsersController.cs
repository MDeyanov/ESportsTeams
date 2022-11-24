using ESportsTeams.Core.Interfaces;
using ESportsTeams.Infrastructure.Data.Common;
using ESportsTeams.Infrastructure.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static ESportsTeams.Infrastructure.Data.Common.CommonConstants;

namespace ESportsTeams.Areas.Administrator.Controllers
{
    public class UsersController : Controller
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
            var currentUserId = _userManager.GetUserAsync(this.User).Result.Id;

            var users = _adminService.GetAllUsers(currentUserId);

            return View(users);
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
    }
}

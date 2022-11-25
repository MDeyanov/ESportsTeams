using ESportsTeams.Infrastructure.Data.Common;
using Microsoft.AspNetCore.Mvc;
using static ESportsTeams.Infrastructure.Data.Common.RedirectConstants;
using static ESportsTeams.Infrastructure.Data.Common.CommonConstants;
using static ESportsTeams.Infrastructure.Data.UserRoles;
using Microsoft.AspNetCore.Authorization;

namespace ESportsTeams.Areas.Administrator.Controllers
{
    [Area(AdminSuffix)]
    [Authorize(Roles = Admin)]
    public class AdminController : Controller
    {
        protected IActionResult RedirectToHome => RedirectToAction(AdminHomeRoute);
    }
}

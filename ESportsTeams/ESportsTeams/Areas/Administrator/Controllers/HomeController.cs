using Microsoft.AspNetCore.Mvc;

namespace ESportsTeams.Areas.Administrator.Controllers
{
    public class HomeController : AdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using ESportsTeams.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESportsTeams.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _eventService.GetAllAsync();
            return View(model);
        }
    }
}

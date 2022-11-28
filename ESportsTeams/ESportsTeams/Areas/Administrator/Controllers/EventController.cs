using ESportsTeams.Core.Interfaces;
using ESportsTeams.Core.Models.BindingModels.Event;
using Microsoft.AspNetCore.Mvc;

namespace ESportsTeams.Areas.Administrator.Controllers
{
    public class EventController : AdminController
    {
        public readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _eventService.GetAllForAdminAsync();
            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEventBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {

                await _eventService.AddEventAsync(model);
                return RedirectToAction("Index", "Event");
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
            var currentEvent = await _eventService.GetEventDetailsByIdAsync(id);
            if (currentEvent == null)
            {
                return RedirectToAction("Index", "Event");
            }
            return View(currentEvent);

        }

        //[HttpPost, ActionName("Delete")]
        //public async Task<IActionResult> DeleteEvent(int id)
        //{
        //    var deleteEvent = await _eventService.DeleteEventAsync(id);
        //    if (!deleteEvent)
        //    {
        //        return View("Error");
        //    }

        //    return RedirectToAction("Index", "Event");
        //}

        [HttpGet]
        public IActionResult ReverseDeletion(int id)
        {
            var result = _eventService.ReverseIsDeleted(id);

            return RedirectToAction("Index", "Event");
        }

        [HttpGet]
        public IActionResult DeleteCurEvent(int id)
        {
            var result = _eventService.Delete(id);

            return RedirectToAction("Index", "Event");
        }
    }
}

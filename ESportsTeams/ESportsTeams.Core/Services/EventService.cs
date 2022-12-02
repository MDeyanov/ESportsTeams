using ESportsTeams.Core.Interfaces;
using ESportsTeams.Core.Models.BindingModels.Event;
using ESportsTeams.Core.Models.ViewModels.EventViewModels;
using ESportsTeams.Infrastructure.Data;
using ESportsTeams.Infrastructure.Data.Entity;
using Microsoft.EntityFrameworkCore;
using static ESportsTeams.Infrastructure.Data.Common.CommonConstants;


namespace ESportsTeams.Core.Services
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly IUserService _userService;
        public EventService(ApplicationDbContext context, IPhotoService photoService, IUserService userService)
        {
            _context = context;
            _photoService = photoService;
            _userService = userService;
        }

        public async Task AddEventAsync(AddEventBindingModel model)
        {
            var result = await _photoService.AddPhotoAsync(model.Image);
            var newEvent = new Event()
            {
                Title = model.Title,
                Description = model.Description,
                Image = result.Url.ToString(),
            };

            await _context.Events.AddAsync(newEvent);
            await _context.SaveChangesAsync();
        }       

        public async Task<bool> DeleteEventAsync(int id)
        {
             var eventToDelete = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            if (eventToDelete == null)
            {
                throw new ArgumentException(EventNotFound);
            }
            eventToDelete.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<IndexEventViewModel>> GetAllAsync()
        {
            var events = await _context.Events
               .Where(x=>x.IsDeleted == false)
               .Include(x => x.Tournaments)
               .ToListAsync();

            return events
                .Select(e => new IndexEventViewModel()
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    ImageUrl = e.Image,
                });
        }

        public async Task<IEnumerable<IndexEventAdminViewModel>> GetAllForAdminAsync()
        {
            var events = await _context.Events
              .Include(x => x.Tournaments)
              .ToListAsync();

            return events
                .Select(e => new IndexEventAdminViewModel()
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    ImageUrl = e.Image,
                    IsDeleted = e.IsDeleted,
                    Tournaments = e.Tournaments,
                });
        }

        public async Task<EventDetailsViewModel> GetEventDetailsByIdAsync(int id)
        {
            var eventById = await _context.Events
              .Include(x => x.Tournaments)
              .FirstOrDefaultAsync(x => x.Id == id);

            if (eventById == null)
            {
                return null;
            }

            var result = new EventDetailsViewModel()
            {
                Id = eventById.Id,
                Title = eventById.Title,
                ImageUrl = eventById.Image,
                Description = eventById.Description,
                Tournaments = eventById.Tournaments,
            };
            return result;

            
        }



        public async Task<Event?> GetEventByTitleAsync(string title)
        {
            return await _context.Events.FirstOrDefaultAsync(x => x.Title == title);
        }

        public int ReverseIsDeleted(int Id)
        {
            var currentEvent = _context.Events.FirstOrDefault(x=>x.Id== Id);

            if (currentEvent == null)
            {
                return -1;
            }

            currentEvent.IsDeleted= false;
            _context.SaveChanges();
            return currentEvent.Id;
        }

        public int Delete(int Id)
        {
            var currentEvent = _context.Events.FirstOrDefault(x => x.Id == Id);

            if (currentEvent == null)
            {
                return -1;
            }
            currentEvent.IsDeleted = true;
            _context.SaveChanges();
            return currentEvent.Id;
        }

        public async Task EditEventAsync(EditEventBindingModel model)
        {
            var eventToEdit = await _context.Events.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (eventToEdit == null)
            {
                throw new ArgumentException(EventNotFound);
            }

            if (model.Image != null)
            {
                var photoResult = await _photoService.AddPhotoAsync(model.Image);
                if (!string.IsNullOrEmpty(eventToEdit.Image))
                {
                    _ = _photoService.DeletePhotoAsync(eventToEdit.Image);
                }
                eventToEdit.Image = photoResult.Url.ToString();
            }


            eventToEdit.Title = model.Title;
            eventToEdit.Description = model.Description;

            await _context.SaveChangesAsync();
        }

        public async Task<Event?> GetEventByIdAsync(int id)
        {
            return await _context.Events.FirstOrDefaultAsync(x=>x.Id== id);
        }
    }
}

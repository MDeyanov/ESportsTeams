using ESportsTeams.Core.Interfaces;
using ESportsTeams.Core.Models.ViewModels.EventViewModels;
using ESportsTeams.Infrastructure.Data;
using ESportsTeams.Infrastructure.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<IndexEventViewModel>> GetAllAsync()
        {
             var events = await _context.Events
                .Include(x=>x.Tournaments)
                .ToListAsync();

            return events
                .Select(e => new IndexEventViewModel()
                {
                    Id = e.Id,
                    Title=e.Title,
                    Description =e.Description,
                    ImageUrl = e.Image,
                    Tournaments = e.Tournaments,
                });
        }

        public async Task<Event?> GetEventByIdAsync(int id)
        {
            var result = await _context.Events
              .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        

        public async Task<Event?> GetEventByTitleAsync(string title)
        {
            return await _context.Events.FirstOrDefaultAsync(x=>x.Title== title);
        }
    }
}

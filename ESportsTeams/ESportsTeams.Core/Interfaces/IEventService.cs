using ESportsTeams.Core.Models.BindingModels.Event;
using ESportsTeams.Core.Models.ViewModels.EventViewModels;
using ESportsTeams.Infrastructure.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Core.Interfaces
{
    public interface IEventService
    {
        Task<EventDetailsViewModel> GetEventDetailsByIdAsync(int id);
        Task<Event?> GetEventByTitleAsync(string title);
        Task AddEventAsync(AddEventBindingModel model);
        Task<IEnumerable<IndexEventViewModel>> GetAllAsync();
        Task<IEnumerable<IndexEventAdminViewModel>> GetAllForAdminAsync();
        Task<bool> DeleteEventAsync (int id);
        int ReverseIsDeleted(int Id);
        int Delete(int Id);
    }
}

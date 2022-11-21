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
        Task<Event?> GetEventByIdAsync(int id);
        Task<Event?> GetEventByTitleAsync(string title);

        Task<IEnumerable<IndexEventViewModel>> GetAllAsync();
    }
}

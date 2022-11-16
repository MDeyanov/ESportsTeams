using ESportsTeams.Core.Models.BindingModels.Team;
using ESportsTeams.Infrastructure.Data.Entity;
using ESportsTeams.Infrastructure.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Core.Interfaces
{
    public interface ITeamService
    {
        Task AddTeamAsync(AddTeamViewModel model,string userId);
        Task<IEnumerable<Team>> GetSliceAsync(int offset, int size);
        Task<IEnumerable<Team>> GetTeamsByCategoryAndSliceAsync(Category category, int offset, int size);
        Task<IEnumerable<Team>> GetTeamByCountry(string country);
        Task<int> GetCountAsync();
        Task<int> GetCountByCategoryAsync(Category category);
    }
}

using ESportsTeams.Core.Models.BindingModels.Team;
using ESportsTeams.Core.Models.ViewModels.TeamViewModels;
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
        Task AddTeamAsync(AddTeamBindingModel model,string userId);
        Task<IEnumerable<Team>> GetSliceAsync(int offset, int size);
        Task<IEnumerable<Team>> GetSliceOfUserOwnedAsync(string userId, int offset, int size);
        Task<IEnumerable<Team>> GetTeamsByCategoryAndSliceAsync(Category category, int offset, int size);
        Task<IEnumerable<Team>> GetOwnedTeams(string userId, Category category, int offset, int size);
        Task<int> GetCountAsync();
        Task<int> GetOwnedTeamCountAsync(string userId);
        Task<int> GetCountByCategoryAsync(Category category);     
        Task<int> GetCountByCategoryOfUserOwnedAsync(string userId,Category category);
        Task<DetailsTeamViewModel?> GetTeamDetailsByIdAsync(int id, string loggedUserId);
        Task<Team?> GetTeamByIdAsync(int id);
        Task EditTeamAsync (EditTeamBindingModel model);
        Task<bool> TeamExistsAsync(string name);
        Task<IEnumerable<GetTeamsViewModel>> GetAllTeamsAsync();
        Task JoinTeam(string userId, int teamId);

        Task ApproveUser(int reqId);
        Task DeclineUser(int reqId);

    }
}

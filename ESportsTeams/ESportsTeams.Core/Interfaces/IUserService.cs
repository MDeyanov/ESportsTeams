using ESportsTeams.Core.Models.ViewModels.UserViewModel;
using ESportsTeams.Infrastructure.Data.Entity;
using ESportsTeams.Infrastructure.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Core.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<Team>> GetUserOwnedTeamsAsync(string userId, Category category, int offset, int size);
        Task<AppUser> FindUserByIdAsync(string userId);

        Task<UserRequestViewModel> GetUserByID(string userId);

        Task<bool> CurrentUserTeamsHaveCategory(string userId, Category category);

    }
}

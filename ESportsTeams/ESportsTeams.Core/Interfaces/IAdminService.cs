using ESportsTeams.Core.Models.ViewModels.UserViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Core.Interfaces
{
    public interface IAdminService
    {
        ICollection<UserConciseViewModel> GetAllUsers(string currentUserId);
        UserDetailsViewModel GetUser(string userId);
        string BanUser(string userId);

        string UnbanUser(string UserId);
    }
}

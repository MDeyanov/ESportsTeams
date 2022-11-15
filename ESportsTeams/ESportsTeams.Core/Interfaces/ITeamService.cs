using ESportsTeams.Core.Models.BindingModels.Team;
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
    }
}

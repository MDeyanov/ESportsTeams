using ESportsTeams.Infrastructure.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Core.Models.ViewModels.UserViewModel
{
    public class UserDetailsViewModel
    {
        public string UserId { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string ProfileImageUrl { get; set; } = null!;
        public bool IsBanned { get; set; }
        public int? Dota2MMR { get; set; }
        public int? CSGOMMR { get; set; }
        public int? PUBGMMR { get; set; }
        public int? LeagueOfLegendsMMR { get; set; }
        public int? VALORANTMMR { get; set; }

        public IList<Team>? OwnedTeams { get; set; }



    }
}

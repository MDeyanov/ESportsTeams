using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Core.Models.ViewModels.UserViewModel
{
    public class UserRequestViewModel
    {
        [Required]
        public string Id { get; set; } = null!;
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        public string ProfileImageUrl { get; set; } = null!;
        public int? Dota2MMR { get; set; }
        public int? CSGOMMR { get; set; }
        public int? PUBGMMR { get; set; }
        public int? LeagueOfLegendsMMR { get; set; }
        public int? VALORANTMMR { get; set; }
    }
}

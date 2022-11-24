﻿using ESportsTeams.Infrastructure.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Core.Models.ViewModels.UserViewModel
{
    public class UserDetailsViewModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ProfileImageUrl { get; set; }
        public bool IsBanned { get; set; }
        public int? Dota2MMR { get; set; }
        public int? CSGOMMR { get; set; }
        public int? PUBGMMR { get; set; }
        public int? LeagueOfLegendsMMR { get; set; }
        public int? VALORANTMMR { get; set; }

        public IList<Team>? OwnedTeams { get; set; }

        public IList<Review>? Reviews { get; set; }


    }
}
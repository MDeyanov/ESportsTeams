using ESportsTeams.Infrastructure.Data.Entity;
using ESportsTeams.Infrastructure.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Core.Models.ViewModels.TeamViewModels
{
    public class DetailsTeamViewModel
    {
        public int Id { get; set; }

        [Required]
        public string loggedUserId { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public Category Category { get; set; }

        public string? Image { get; set; }

        public int AddressId { get; set; }
        public Address Address { get; set; } = null!;

        public int TournamentWin { get; set; } = 0;


        [Required]
        public string OwnerId { get; set; } = null!;
        public AppUser Owner { get; set; } = null!;

        public List<TeamTournament> TeamTournaments { get; set; } = new List<TeamTournament>();

        public List<Request> Requests { get; set; } = new List<Request> ();
        public List<AppUser> Players { get; set; } = new List<AppUser>();


        public int AvarageMMR { get; set; }

        public Dictionary<string,string> RequestersNames { get; set; } = new Dictionary<string, string> ();
    }
}

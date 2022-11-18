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

        [Required]
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public Category Category { get; set; }

        public string? Image { get; set; }

        public int AddressId { get; set; }
        public Address Address { get; set; } = null!;

        public int TournamentWin { get; set; } = 0;

        // adding a captain of a team

        [Required]
        public string OwnerId { get; set; } = null!;
        public AppUser Owner { get; set; } = null!;

        public List<TeamTournament> TeamTournaments { get; set; } = new List<TeamTournament>();

        public int AvarageMMR { get; set; }
    }
}

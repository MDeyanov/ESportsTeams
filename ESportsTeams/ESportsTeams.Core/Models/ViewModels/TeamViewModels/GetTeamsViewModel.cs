using ESportsTeams.Infrastructure.Data.Entity;
using ESportsTeams.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;


namespace ESportsTeams.Core.Models.ViewModels.TeamViewModels
{
    public class GetTeamsViewModel
    {
        public int Id { get; set; }        

        [Required]
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public Category Category { get; set; }

        public string? Image { get; set; }

        public Address Address { get; set; } = null!;

        public int TournamentWin { get; set; } = 0;

        [Required]
        public AppUser Owner { get; set; } = null!;

        [Required]
        public List<TeamTournament> TeamTournaments { get; set; } = null!;

        public int AvarageMMR { get; set; }
        public bool IsBanned { get; set; }
    }
}

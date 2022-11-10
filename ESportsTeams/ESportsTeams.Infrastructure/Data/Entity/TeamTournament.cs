using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESportsTeams.Infrastructure.Data.Entity
{
    public class TeamTournament
    {
        [Required]
        public int TournamentId { get; set; }

        [ForeignKey(nameof(TournamentId))]
        public Tournament Tournament { get; set; } = null!;

        [Required]
        public int TeamId { get; set; } 

        [ForeignKey(nameof(TeamId))]
        public Team Team { get; set; } = null!;

       
    }
}

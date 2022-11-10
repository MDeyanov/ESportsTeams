using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static ESportsTeams.Infrastructure.Data.Common.ValidationConstants.TeamConstraints;

namespace ESportsTeams.Infrastructure.Data.Entity
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(TeamNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        public string? Image { get; set; }
        //[ForeignKey("Address")]
        public int? AddressId { get; set; }

        [ForeignKey(nameof(AddressId))]
        public Address? Address { get; set; }
        //
        //[ForeignKey("AppUser")]
        public string? AppUserId { get; set; }

        [ForeignKey(nameof(AppUserId))]
        public IList<AppUser>? AppUsers { get; set; } = new List<AppUser>();
        public int TournamentWin { get; set; } = 0;

        

        public List<TeamTournament> TeamTournaments { get; set; } = new List<TeamTournament>();

    }
}

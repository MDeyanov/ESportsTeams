using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static ESportsTeams.Infrastructure.Data.Common.ValidationConstants.TeamConstraints;
using ESportsTeams.Infrastructure.Data.Enums;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

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

        [Required]
        public Category Category { get; set; }

        public string? Image { get; set; }

        public int AddressId { get; set; }

        [Required]
        [ForeignKey(nameof(AddressId))]
        public Address Address { get; set; } = null!;

        public IList<AppUser>? AppUsers { get; set; } = new List<AppUser>();
        public int TournamentWin { get; set; } = 0;


        [Required]
        public string OwnerId { get; set; } = null!;

        [ForeignKey(nameof(OwnerId))]
        public AppUser Owner { get; set; } = null!;

        [Required]
        public bool IsBanned { get; set; } = false;

        public List<TeamTournament> TeamTournaments { get; set; } = new List<TeamTournament>();

        public int AvarageMMR { get; set; }

    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using static ESportsTeams.Infrastructure.Data.Common.ValidationConstants.TournamentConstraints;

namespace ESportsTeams.Infrastructure.Data.Entity
{
    public class Tournament
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        public string? Image { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Column(TypeName = "money")]
        [Precision(18, 2)]
        public decimal? EntryFee { get; set; }

        public string? Website { get; set; }
        public string? Twitter { get; set; }
        public string? Facebook { get; set; }
        public string? Contact { get; set; }

        [Column(TypeName = "money")]
        [Precision(18, 2)]
        public decimal? PrizePool { get; set; }

        public int? AddressId { get; set; }

        [ForeignKey(nameof(AddressId))]
        public Address? Address { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        [ForeignKey(nameof(EventId))]
        public Event Event { get; set; } = null!;
        
        public IList<TeamTournament> TeamTournaments { get; set; } = new List<TeamTournament>();
       
        public IList<Review>? Reviews { get; set; } = new List<Review>();

        [Required]
        public bool IsDeleted { get; set; } = false;
    }
}

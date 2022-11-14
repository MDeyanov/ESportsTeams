using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ESportsTeams.Infrastructure.Data.Common.ValidationConstants.ReviewConstrains;

namespace ESportsTeams.Infrastructure.Data.Entity
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(ContentMessageMaxLength)]
        public string Content { get; set; } = null!;

        [Required]
        public string AppUserId { get; set; } = null!;

        [ForeignKey(nameof(AppUserId))]
        public AppUser AppUser { get; set; } = null!;

        [Required]
        public int? TournamentId { get; set; }

        [ForeignKey(nameof(TournamentId))]
        public Tournament? Tournament { get; set; } = null!;


        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ESportsTeams.Infrastructure.Data.Common.ValidationConstants.EventConstrains;

namespace ESportsTeams.Infrastructure.Data.Entity
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(EventTitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(EventDescriptionMaxLength)]
        public string Description { get; set; } = null!;

        public IList<Tournament>? Tournaments { get; set; } = new List<Tournament>();  
    }
}

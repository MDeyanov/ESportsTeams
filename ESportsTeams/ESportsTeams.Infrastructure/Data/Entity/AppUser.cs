using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ESportsTeams.Infrastructure.Data.Common.ValidationConstants.UserConstraints;


namespace ESportsTeams.Infrastructure.Data.Entity
{
    public class AppUser : IdentityUser
    {
        [Required]
        [StringLength(FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(LastNameMaxLength)]
        public string LastName { get; set; } = null!;

        public string? ProfileImageUrl { get; set; }      

        public int? AddressId { get; set; }

        [ForeignKey(nameof(AddressId))]
        public Address? Address { get; set; }

        public int? TeamId { get; set; }

        [ForeignKey(nameof(TeamId))]
        public Team? Team { get; set; }

        // Games Match Making Rating
        public int? Dota2MMR { get; set; }
        public int? CSGOMMR { get; set; }
        public int? PUBGMMR { get; set; }
        public int? LeagueOfLegendsMMR { get; set; }
        public int? VALORANTMMR { get; set; }
        
        public IList<Team>? OwnedTeams { get; set; } = new List<Team>();
        public bool IsBanned { get; set; } = false;
    }
}

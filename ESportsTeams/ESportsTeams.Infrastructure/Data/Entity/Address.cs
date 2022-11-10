using System.ComponentModel.DataAnnotations;

namespace ESportsTeams.Infrastructure.Data.Entity
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Street { get; set; } = null!;

        [Required]
        public string City { get; set; } = null!;

        [Required]
        public string Country { get; set; } = null!;

        public int ZipCode { get; set; }

        //public List<Team> Teams { get; set; } = new List<Team>();
        //public List<Tournament> Tournaments { get; set; } = new List<Tournament>();
    }
}

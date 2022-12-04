using ESportsTeams.Infrastructure.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Infrastructure.Data.Entity
{
    public class Request
    {
        [Key]
        public int Id { get; set; }
        public RequestStatus Status { get; set; }

        public int TeamId { get; set; }

        [Required]
        [ForeignKey(nameof(TeamId))]
        public Team Team { get; set; } = null!;

        [Required]
        public string RequesterId { get; set; } = null!;

        

    }
}

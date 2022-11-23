using ESportsTeams.Infrastructure.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Core.Models.ViewModels.TeamViewModels
{
    public class OwnTeamsViewModel
    {
        [Required]
        public string loggedUserId { get; set; } = null!;
        public IEnumerable<Team>? Teams { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalTeams { get; set; }
        public int Category { get; set; }
        public bool HasPreviousPage => Page > 1;

        public bool HasNextPage => Page < TotalPages;
    }
}

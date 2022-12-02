using ESportsTeams.Infrastructure.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Core.Models.ViewModels.TournamentViewModels
{
    public class TournamentDetailsViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }

        public string? Description { get; set; }
        public string? Image { get; set; }

        public DateTime StartTime { get; set; }

        public decimal? EntryFee { get; set; }

        public string? Website { get; set; }
        public string? Twitter { get; set; }
        public string? Facebook { get; set; }
        public string? Contact { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal? PrizePool { get; set; }

        [Required]
        public string EventTitle { get; set; } = null!;

        [Required]
        public Address Address { get; set; } = null!;
        public IList<TeamTournament>? TeamTournaments { get; set; }
        public IList<Review>? Reviews { get; set; }
    }
}

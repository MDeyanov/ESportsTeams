using ESportsTeams.Infrastructure.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Core.Models.ViewModels.TournamentViewModels
{
    public class TournamentViewModel
    {
        // tova da go napravq malko po kratko i pylnata informaciq shte davam v details page
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
        public Address? Address { get; set; }
        public IList<TeamTournament>? TeamTournaments { get; set; }

    }
}

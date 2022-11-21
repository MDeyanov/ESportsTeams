using ESportsTeams.Infrastructure.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Core.Models.ViewModels.TournamentViewModels
{
    public class IndexTournamentViewModel
    {
        public IEnumerable<Tournament>? Tournaments { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalTournaments { get; set; }
        public int EventsCount { get; set; }
        public bool HasPreviousPage => Page > 1;

        public bool HasNextPage => Page < TotalPages;
    }
}

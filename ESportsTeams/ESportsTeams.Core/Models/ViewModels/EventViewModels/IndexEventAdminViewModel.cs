using ESportsTeams.Infrastructure.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Core.Models.ViewModels.EventViewModels
{
    public class IndexEventAdminViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsDeleted { get; set; }

        public IList<Tournament>? Tournaments { get; set; }
    }
}

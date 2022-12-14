using ESportsTeams.Infrastructure.Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ESportsTeams.Infrastructure.Data.Common.ValidationConstants.TournamentConstraints;

namespace ESportsTeams.Core.Models.BindingModels.Tournament
{
    public class EditTournamentBindingModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;


        [Required]
        public DateTime StartTime { get; set; }

        [Precision(18, 2)]
        public decimal? EntryFee { get; set; }

        public string? Website { get; set; }
        public string? Twitter { get; set; }
        public string? Facebook { get; set; }
        public string? Contact { get; set; }

        [Column(TypeName = "money")]
        [Precision(18, 2)]
        public decimal? PrizePool { get; set; }


        
        public IFormFile? Image { get; set; }

        [Required]
        public Address Address { get; set; } = null!;

        public string? EventTitle { get; set; }
    }
}

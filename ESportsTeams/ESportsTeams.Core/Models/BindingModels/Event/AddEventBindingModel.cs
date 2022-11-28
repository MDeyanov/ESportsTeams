using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ESportsTeams.Infrastructure.Data.Common.ValidationConstants.EventConstrains;

namespace ESportsTeams.Core.Models.BindingModels.Event
{
    public class AddEventBindingModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(EventTitleMaxLength, MinimumLength = EventTitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        public IFormFile Image { get; set; } = null!;

        [Required]
        [StringLength(EventDescriptionMaxLength,MinimumLength = EventDescriptionMinLength)]
        public string Description { get; set; } = null!;
    }
}

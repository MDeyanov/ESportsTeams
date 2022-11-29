using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Core.Models.BindingModels.Event
{
    public class EditEventBindingModel
    {
      
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public IFormFile Image { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        public string? URL { get; set; }
    }
}

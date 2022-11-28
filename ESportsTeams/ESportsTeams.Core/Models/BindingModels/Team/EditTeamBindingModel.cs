using ESportsTeams.Infrastructure.Data.Entity;
using ESportsTeams.Infrastructure.Data.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ESportsTeams.Infrastructure.Data.Common.ValidationConstants.TeamConstraints;


namespace ESportsTeams.Core.Models.BindingModels.Team
{
    public class EditTeamBindingModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(TeamNameMaxLength, MinimumLength = TeamNameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;

        public int AddressId { get; set; }
        public Address? Address { get; set; }

        public IFormFile Image { get; set; }

        public string? URL { get; set; }

        [Required]
        public Category Category { get; set; }


    }
}

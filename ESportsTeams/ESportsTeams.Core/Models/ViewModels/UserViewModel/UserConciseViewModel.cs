using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Core.Models.ViewModels.UserViewModel
{
    public class UserConciseViewModel
    {
        [Required]
        public string Id { get; set; } = null!;
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public bool IsBanned { get; set; }
    }
}

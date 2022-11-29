// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ESportsTeams.Core.Interfaces;
using ESportsTeams.Infrastructure.Data;
using ESportsTeams.Infrastructure.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESportsTeams.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;

        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ApplicationDbContext context,
            IPhotoService photoService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _photoService = photoService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Display(Name = "Dota2 MMR")]
            public int? Dota2MMR { get; set; }

            [Display(Name = "CSGO MMR")]
            public int? CSGOMMR { get; set; }

            [Display(Name = "PUBG MMR")]
            public int? PUBGMMR { get; set; }

            [Display(Name = "LeagueOfLegends MMR")]
            public int? LeagueOfLegendsMMR { get; set; }

            [Display(Name = " VALORANT MMR")]
            public int? VALORANTMMR { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
           
            public string ProfileImageUrl { get; set; }

            [Display(Name = "Profile picture")]
            public IFormFile Image { get; set; }
        }

        private async Task LoadAsync(AppUser user)
        {          
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var dota2Mmr = user.Dota2MMR;
            var csgoMmr = user.CSGOMMR;
            var pubgMmr = user.PUBGMMR;
            var LeagueOfLegendsMmr = user.LeagueOfLegendsMMR;
            var valorantMmr = user.VALORANTMMR;
            var profileImageUrl = user.ProfileImageUrl;
           
            Username = userName;


            Input = new InputModel
            {
                Dota2MMR = dota2Mmr,
                CSGOMMR = csgoMmr,
                PUBGMMR = pubgMmr,
                LeagueOfLegendsMMR = LeagueOfLegendsMmr,
                VALORANTMMR = valorantMmr,
                PhoneNumber = phoneNumber,
                ProfileImageUrl = profileImageUrl
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            if (Input.Dota2MMR != user.Dota2MMR)
            {
                user.Dota2MMR = Input.Dota2MMR;

            }
            if (Input.VALORANTMMR != user.VALORANTMMR)
            {
                user.VALORANTMMR = Input.VALORANTMMR;

            }
            if (Input.CSGOMMR != user.CSGOMMR)
            {
                user.CSGOMMR = Input.CSGOMMR;

            }
            if (Input.LeagueOfLegendsMMR != user.LeagueOfLegendsMMR)
            {
                user.LeagueOfLegendsMMR = Input.LeagueOfLegendsMMR;
            }
            if (Input.PUBGMMR != user.PUBGMMR)
            {
                user.PUBGMMR = Input.PUBGMMR;
            }
            if (Input.Image != null)
            {
                var photoResult = await _photoService.AddPhotoAsync(Input.Image);
                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    _ = _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }
                user.ProfileImageUrl = photoResult.Url.ToString();
            }


            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            await _context.SaveChangesAsync();
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}

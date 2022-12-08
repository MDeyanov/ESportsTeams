// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using ESportsTeams.Controllers;
using ESportsTeams.Core.Interfaces;
using ESportsTeams.Infrastructure.Data.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using static ESportsTeams.Infrastructure.Data.Common.ValidationConstants.UserConstraints;

namespace ESportsTeams.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IPhotoService _photoService;

        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IPhotoService photoService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _photoService = photoService;
        }


        [BindProperty]
        public InputModel Input { get; set; }


        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Username")]
            [StringLength(UserNameMaxLength,
                ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = UserNameMinLength)]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "First Name")]
            [StringLength(FirstNameMaxLength,
                ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = FirstNameMinLength)]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            [StringLength(LastNameMaxLength,
                ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = LastNameMinLength)]
            public string LastName { get; set; }

            [Display(Name = "Your Dota2 MMR")]
            public int? Dota2MMR { get; set; }

            [Display(Name = "Your CSGO MMR")]
            public int? CSGOMMR { get; set; }

            [Display(Name = "Your PUBG MMR")]
            public int? PUBGMMR { get; set; }

            [Display(Name = "Your LeagueOfLegends MMR")]
            public int? LeagueOfLegendsMMR { get; set; }

            [Display(Name = "Your VALORANT MMR")]
            public int? VALORANTMMR { get; set; }

            public IFormFile ProfileImage { get; set; }     

            [Required]
            [StringLength(PasswordMaxLength,
                ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = PasswordMinLength)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }


            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public void OnGet(string returnUrl = null)
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                Response.Redirect("/");
            }
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                ImageUploadResult photoResult = null;
                if (Input.ProfileImage != null) 
                {
                    photoResult = await _photoService.AddPhotoAsync(Input.ProfileImage);

                }
                var user = new AppUser
                {
                    UserName = Input.Username,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    CSGOMMR = Input.CSGOMMR,
                    Dota2MMR = Input.Dota2MMR,
                    LeagueOfLegendsMMR = Input.LeagueOfLegendsMMR,
                    VALORANTMMR = Input.VALORANTMMR,
                    PUBGMMR = Input.PUBGMMR,
                    ProfileImageUrl = photoResult?.Url.ToString(),
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                 
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Redirect("/Home/Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

    }
}

﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplicatationPollService.Models.ViewModels {
    public class LoginViewModel
    {
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 5, ErrorMessage = "Login must be at least 5 and at most 20 characters long.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 5 , ErrorMessage ="Login must be at least 5 and at most 20 characters long." )]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

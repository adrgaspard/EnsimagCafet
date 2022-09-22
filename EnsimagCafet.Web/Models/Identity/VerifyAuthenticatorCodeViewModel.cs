﻿using System.ComponentModel.DataAnnotations;

namespace EnsimagCafet.Web.Models.Identity
{
    public sealed class VerifyAuthenticatorCodeViewModel
    {
        [Required]
        public string Code { get; set; } = "";

        public string? ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
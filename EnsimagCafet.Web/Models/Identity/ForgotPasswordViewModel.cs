using System.ComponentModel.DataAnnotations;

namespace EnsimagCafet.Web.Models.Identity
{
    public sealed class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}
using System.ComponentModel.DataAnnotations;

namespace EnsimagCafet.Web.Models.Identity
{
    public sealed class UseRecoveryCodeViewModel
    {
        [Required]
        public string Code { get; set; } = "";

        public string? ReturnUrl { get; set; }
    }
}

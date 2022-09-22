using System.ComponentModel.DataAnnotations;

namespace EnsimagCafet.Web.Models.Identity
{
    public sealed class LoginViewModel
    {
        [Required]
        public string Login { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
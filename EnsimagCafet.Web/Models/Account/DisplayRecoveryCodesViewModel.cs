using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnsimagCafet.Web.Models.Account
{
    public sealed class DisplayRecoveryCodesViewModel
    {
        [Required]
        public IEnumerable<string> Codes { get; set; } = Enumerable.Empty<string>();

    }
}

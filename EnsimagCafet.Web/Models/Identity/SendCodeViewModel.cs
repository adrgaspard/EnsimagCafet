using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EnsimagCafet.Web.Models.Identity
{
    public sealed class SendCodeViewModel
    {
        public string? SelectedProvider { get; set; }

        public ICollection<SelectListItem> Providers { get; set; } = Array.Empty<SelectListItem>();

        public string? ReturnUrl { get; set; }

        public bool RememberMe { get; set; }
    }
}

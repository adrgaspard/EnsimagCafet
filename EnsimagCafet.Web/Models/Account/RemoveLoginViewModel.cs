using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnsimagCafet.Web.Models.Account
{
    public sealed class RemoveLoginViewModel
    {
        public string? LoginProvider { get; set; }
        public string? ProviderKey { get; set; }
    }
}

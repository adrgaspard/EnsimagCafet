using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace EnsimagCafet.Web.Models.Account
{
    public sealed class IndexViewModel
    {
        public bool HasPassword { get; set; }

        public IList<UserLoginInfo> Logins { get; set; } = new List<UserLoginInfo>(0);

        public string? PhoneNumber { get; set; }

        public bool TwoFactor { get; set; }

        public bool BrowserRemembered { get; set; }

        public string? AuthenticatorKey { get; set; }
    }
}

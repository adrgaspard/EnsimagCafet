using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace EnsimagCafet.Web.Models.Account
{
    public sealed class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; } = new List<UserLoginInfo>(0);

        public IList<AuthenticationScheme> OtherLogins { get; set; } = new List<AuthenticationScheme>(0);
    }
}
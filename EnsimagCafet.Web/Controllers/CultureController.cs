using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace EnsimagCafet.Web.Controllers
{
    [Route("[controller]")]
    public sealed class CultureController : Controller
    {
        [HttpPost("set")]
        [AllowAnonymous]
        public IActionResult Set(string culture, string redirectUri)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new(culture)));
            return LocalRedirect(redirectUri);
        }
    }
}
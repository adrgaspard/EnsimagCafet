﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace EnsimagCafet.Web.Controllers
{
    [Route("[controller]")]
    [AllowAnonymous]
    public sealed class CultureController : Controller
    {
        [HttpPost("set")]
        public IActionResult Set([FromForm] string culture, [FromQuery] string redirectUri)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new(culture)));
            return LocalRedirect(redirectUri);
        }
    }
}

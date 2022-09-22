using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnsimagCafet.Web.Controllers
{
    [Route("[controller]")]
    public sealed class ErrorController : Controller
    {
        [HttpGet("{statusCode}")]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult HttpError([FromRoute] int statusCode)
        {
            if (statusCode < 400 || statusCode > 599)
            {
                return LocalRedirect("~/Error/404");
            }
            ViewData["Code"] = statusCode;
            return View("Index");
        }
    }
}

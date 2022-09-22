using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;

namespace EnsimagCafet.Web.Controllers
{
    public sealed class HomeController : Controller
    {
        private readonly IHtmlLocalizer<HomeController> _localizer;

        public HomeController(IHtmlLocalizer<HomeController> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet("")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            ViewData["Test"] = _localizer["TestC"];
            return View("index");
        }

        [HttpGet("privacy")]
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View("privacy");
        }

        [HttpGet("bootstrap")]
        [AllowAnonymous]
        public IActionResult Bootstrap()
        {
            return View("bootstrap");
        }
    }
}
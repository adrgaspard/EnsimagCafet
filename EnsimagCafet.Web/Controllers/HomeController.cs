using EnsimagCafet.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using System.Diagnostics;

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
            return View();
        }

        [HttpGet("privacy")]
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
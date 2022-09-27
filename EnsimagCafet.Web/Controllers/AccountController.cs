using EnsimagCafet.Domain.Identity;
using EnsimagCafet.Web.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnsimagCafet.Web.Controllers
{
    [Route("[controller]")]
    public sealed class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            Error
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await _userManager.GetUserAsync(HttpContext.User); ;
            var model = new IndexViewModel
            {
                HasPassword = await _userManager.HasPasswordAsync(user),
                TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
                BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
            };
            return View("Index", model);
        }

        [HttpPost("EnableTwoFactorAuthentication")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableTwoFactorAuthentication()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User); ;
            if (user is not null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, true);
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation(1, "User enabled two-factor authentication.");
            }
            return RedirectToAction(nameof(Index), "Account");
        }

        [HttpPost("DisableTwoFactorAuthentication")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisableTwoFactorAuthentication()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User); ;
            if (user is not null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, false);
                await _signInManager.SignInAsync(user, false);
                _logger.LogInformation(2, "User disabled two-factor authentication.");
            }
            return RedirectToAction(nameof(Index), "Account");
        }

        [HttpGet("ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View("ChangePassword");
        }

        [HttpPost("ChangePassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ChangePassword", model);
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user is not null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    _logger.LogInformation(3, "User changed their password successfully.");
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                AddErrors(result);
                return View("ChangePassword", model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        [HttpGet("SetPassword")]
        public IActionResult SetPassword()
        {
            return View("SetPassword");
        }

        [HttpPost("SetPassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SetPassword", model);
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user is not null)
            {
                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
                return View("SetPassword", model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
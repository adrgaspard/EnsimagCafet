using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EnsimagCafet.Domain.Identity;
using EnsimagCafet.Web.Models.Identity;
using EnsimagCafet.Web.Models.Account;

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

        [HttpGet("Index")]
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
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
                Logins = await _userManager.GetLoginsAsync(user),
                BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
                AuthenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user)
            };
            return View("Index", model);
        }

        [HttpPost("RemoveLogin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel account)
        {
            ManageMessageId? message = ManageMessageId.Error;
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user is not null)
            {
                var result = await _userManager.RemoveLoginAsync(user, account.LoginProvider, account.ProviderKey);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    message = ManageMessageId.RemoveLoginSuccess;
                }
            }
            return RedirectToAction(nameof(ManageLogins), new { Message = message });
        }

        [HttpPost("ResetAuthenticatorKey")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetAuthenticatorKey()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                _logger.LogInformation(1, "User reset authenticator key.");
            }
            return RedirectToAction(nameof(Index), "Account");
        }

        [HttpPost("GenerateRecoveryCode")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateRecoveryCode()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user is not null)
            {
                var codes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 5);
                _logger.LogInformation(1, "User generated new recovery code.");
                return View("DisplayRecoveryCodes", new DisplayRecoveryCodesViewModel { Codes = codes });
            }
            return Unauthorized();
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

        [HttpGet("ManageLogins")]
        public async Task<IActionResult> ManageLogins(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.AddLoginSuccess ? "The external login was added."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user is null)
            {
                return Unauthorized();
            }
            var userLogins = await _userManager.GetLoginsAsync(user);
            var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();
            var otherLogins = schemes.Where(auth => userLogins.All(userLogin => auth.Name != userLogin.LoginProvider)).ToList();
            ViewData["ShowRemoveButton"] = user.PasswordHash != null || userLogins.Count > 1;
            return View("ManageLogins", new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        [HttpPost("LinkLogin")]
        [ValidateAntiForgeryToken]
        public IActionResult LinkLogin(string provider)
        {
            var redirectUrl = Url.Action("LinkLoginCallback", "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return Challenge(properties, provider);
        }

        [HttpGet("LinkLoginCallback")]
        public async Task<ActionResult> LinkLoginCallback()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user is null)
            {
                return Unauthorized();
            }
            var info = await _signInManager.GetExternalLoginInfoAsync(await _userManager.GetUserIdAsync(user));
            if (info is null)
            {
                return RedirectToAction(nameof(ManageLogins), new { Message = ManageMessageId.Error });
            }
            var result = await _userManager.AddLoginAsync(user, info);
            var message = result.Succeeded ? ManageMessageId.AddLoginSuccess : ManageMessageId.Error;
            return RedirectToAction(nameof(ManageLogins), new { Message = message });
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public enum ManageMessageId
        {
            AddLoginSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

    }
}

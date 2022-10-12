using APITools.CommonTools;
using EnsimagCafet.Domain.Identity;
using EnsimagCafet.Web.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace EnsimagCafet.Web.Controllers
{
    [Route("[controller]")]
    public sealed class IdentityController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<IdentityController> _logger;

        public IdentityController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender, ILogger<IdentityController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [HttpGet("Login")]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View("Login");
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, model.RememberMe, true);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View("Login", model);
                }
            }
            return View("Login", model);
        }

        [HttpGet("Register")]
        [AllowAnonymous]
        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                model.Email = model.Email.ToLower();
                var userResult = Domain.Identity.User.Instanciate(model.Email, "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
                if (userResult)
                {
                    var user = userResult.Value;
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Action("ConfirmEmail", "Identity", new { userId = user.Id, code }, protocol: HttpContext.Request.Scheme);
                        await _emailSender.SendEmailAsync(model.Email, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                        _logger.LogInformation(3, "User created a new account with password.");
                        return View("VerifyEmail");
                    }
                    AddErrors(result);
                }
                AddErrors(userResult);
            }
            return View("Register", model);
        }

        [HttpPost("LogOff")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return LocalRedirect("~/");
        }

        [HttpGet("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId is null || code is null)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return Unauthorized();
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet("ForgotPassword")]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return View("ForgotPasswordConfirmation");
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Identity", new { userId = user.Id, code }, protocol: HttpContext.Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                return View("ForgotPasswordConfirmation");
            }
            return View("ForgotPassword", model);
        }

        [HttpGet("ForgotPasswordConfirmation")]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View("ForgotPasswordConfirmation");
        }

        [HttpGet("ResetPassword")]
        [AllowAnonymous]
        public IActionResult ResetPassword(string? code = null)
        {
            return code == null ? BadRequest() : View("ResetPassword");
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                return RedirectToAction(nameof(IdentityController.ResetPasswordConfirmation), "Identity");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(IdentityController.ResetPasswordConfirmation), "Identity");
            }
            AddErrors(result);
            return View("ResetPassword");
        }

        [HttpGet("ResetPasswordConfirmation")]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View("ResetPasswordConfirmation");
        }

        [HttpGet("SendCode")]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string? returnUrl = null, bool rememberMe = false)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user is null)
            {
                return Unauthorized();
            }
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View("SendCode", new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost("SendCode")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SendCode");
            }
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user is null)
            {
                return Unauthorized();
            }
            if (model.SelectedProvider == "Authenticator")
            {
                return RedirectToAction(nameof(VerifyAuthenticatorCode), new { model.ReturnUrl, model.RememberMe });
            }
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
            if (string.IsNullOrWhiteSpace(code))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            var message = "Your security code is: " + code;
            if (model.SelectedProvider == "Email")
            {
                await _emailSender.SendEmailAsync(await _userManager.GetEmailAsync(user), "Security Code", message);
            }
            return RedirectToAction(nameof(VerifyCode), new { Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe });
        }

        [HttpGet("VerifyCode")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode(string provider, bool rememberMe, string? returnUrl = null)
        {
            if (await _signInManager.GetTwoFactorAuthenticationUserAsync() is null)
            {
                return Unauthorized();
            }
            return View("VerifyCode", new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost("VerifyCode")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("VerifyCode", model);
            }
            var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
            if (result.Succeeded)
            {
                return RedirectToLocal(model.ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning(7, "User account locked out.");
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid code.");
                return View("VerifyCode", model);
            }
        }

        [HttpGet("VerifyAuthenticatorCode")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyAuthenticatorCode(bool rememberMe, string? returnUrl = null)
        {
            if (await _signInManager.GetTwoFactorAuthenticationUserAsync() is null)
            {
                return Unauthorized();
            }
            return View("VerifyCode", new VerifyAuthenticatorCodeViewModel { ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost("VerifyAuthenticatorCode")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyAuthenticatorCode(VerifyAuthenticatorCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("VerifyAuthenticatorCode", model);
            }
            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(model.Code, model.RememberMe, model.RememberBrowser);
            if (result.Succeeded)
            {
                return RedirectToLocal(model.ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning(7, "User account locked out.");
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid code.");
                return View("VerifyAuthenticatorCode", model);
            }
        }

        [HttpGet("UseRecoveryCode")]
        [AllowAnonymous]
        public async Task<IActionResult> UseRecoveryCode(string? returnUrl = null)
        {
            if (await _signInManager.GetTwoFactorAuthenticationUserAsync() is null)
            {
                return Unauthorized();
            }
            return View("UseRecoveryCode", new UseRecoveryCodeViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost("UseRecoveryCode")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UseRecoveryCode(UseRecoveryCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("UseRecoveryCode", model);
            }

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(model.Code);
            if (result.Succeeded)
            {
                return RedirectToLocal(model.ReturnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid code.");
                return View("UseRecoveryCode", model);
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private void AddErrors(Result result)
        {
            if (result.Exception is CompositeException compositeException)
            {
                foreach (var subException in compositeException.InnerExceptions)
                {
                    ModelState.AddModelError(string.Empty, subException.Message);
                }
            }
            else if (result.Exception is Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
        }

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            returnUrl ??= "";
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return LocalRedirect("~/");
            }
        }
    }
}
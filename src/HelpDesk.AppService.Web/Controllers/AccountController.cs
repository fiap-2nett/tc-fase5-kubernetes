using System;
using System.Threading.Tasks;
using HelpDesk.AppService.Application.Core.Abstractions.Services;
using HelpDesk.AppService.Web.Extensions;
using HelpDesk.AppService.Web.Models.AccountViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.AppService.Web.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        #region Read-Only Fields

        private readonly IAccountService _accountService;

        #endregion

        #region Constructors

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        #endregion

        #region Controller Actions

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {            
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {                
                var result = await _accountService.PasswordSignInAsync(model.Email, model.Password, model.RememberMe);
                if (result.IsSuccess)
                    return RedirectToLocal(returnUrl);

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
            
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await _accountService.RegisterAsync(model.Name, model.Surname, model.Email, model.Password);
                if (result.IsSuccess)
                    return RedirectToLocal(returnUrl);

                ModelState.AddErrors(result.Errors);
            }

            return View(model);
        }

        [HttpGet]        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var result = await _accountService.GetAsync();
            if (!result.IsSuccess)
                throw new Exception("An error occurred while processing the request.");

            var model = (SettingsViewModel)result.User;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(SettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.UpdateAsync(model.Name, model.Surname);
                if (!result.IsSuccess)
                    ModelState.AddErrors(result.Errors);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var result = await _accountService.GetAsync();
            if (!result.IsSuccess)
                throw new Exception("An error occurred while processing the request.");

            var model = new ChangePasswordViewModel { Email = result.User.Email };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.ChangePasswordAsync(model.Password);
                if (result.IsSuccess)
                    return RedirectToAction(nameof(AccountController.Logout));

                ModelState.AddErrors(result.Errors);
            }

            return View(model);
        }

        #endregion

        #region Private Methods

        private IActionResult RedirectToLocal(string returnUrl)
            => Url.IsLocalUrl(returnUrl)
                ? Redirect(returnUrl)
                : RedirectToAction(nameof(HomeController.Index), "Home");

        #endregion
    }
}

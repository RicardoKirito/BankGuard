using BankGuard.Core.Application.Dtos.Account;
using BankGuard.Core.Application.Helpers;
using BankGuard.Core.Application.Interfaces.Services;
using BankGuard.Core.Application.ViewModels.User;
using BankGuard.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace BankGuard.Controllers
{
    public class UserController : Controller
    {
        private readonly IAccountServices _userServices;
        public UserController(IAccountServices userServices)
        {
            _userServices = userServices;
        }
        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            AuthenticationResponse response = await _userServices.LoginAsync(login);
            if(response != null && response.HasError != true)
            {
                HttpContext.Session.Set<AuthenticationResponse>("user", response);
                if (response.Role[0].ToString() == "Admin")
                {
                    return RedirectToRoute(new { controller = "Admin", action = "Home" });
                }
                return RedirectToRoute(new { controller = "Basic", action = "Index" });
            }
            else
            {
                login.ErrorMessage = response.ErrorMessage;
                login.HasError = response.HasError;
                return View(login);
            }

        }
        public async Task<IActionResult> LogOut()
        {
            await _userServices.SignOut();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Login" });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> ConfirmEmail(string Userid, string token)
        {
            string response = await _userServices.ConfirmEmailAsync(Userid, token);
            return View("ConfirmEmail", response);
        }
        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var origin = Request.Headers["origin"];
            ForgotPasswordResponse response = await _userServices.ForgotPasswordAsync(vm, origin);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }
            return RedirectToRoute(new { controller = "User", action = "Login" });

        }
        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult ResetPassword()
        {
            return View(new ResetPasswordViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                View(vm);
            }
            ResetPasswordResponse response = await _userServices.ResetPasswordAsync(vm);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }

            return RedirectToRoute(new { controller = "User", action = "Login" });

        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

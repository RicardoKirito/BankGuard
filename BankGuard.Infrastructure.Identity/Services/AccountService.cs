using BankGuard.Core.Application.Dtos.Account;
using BankGuard.Core.Application.Enums;
using BankGuard.Core.Application.Interfaces.Services;
using BankGuard.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.WebEncoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new AuthenticationResponse();

            var userbyName = await _userManager.FindByNameAsync(request.Email);
            var userbyEmail = await _userManager.FindByEmailAsync(request.Email);
            var user = userbyName == null ? userbyEmail : userbyName;
            if (user == null)
            {
                response.HasError = true;
                response.ErrorMessage = $"There is not account registered with \"{request.Email}\"";
                return response;
            }
            
            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.ErrorMessage = $"Invalid password";
            }
            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.ErrorMessage = $"This Account is inactive, need to confirm to sign in.";
                response.IsVerified = user.EmailConfirmed;

                return response;
            }

            response.Id = user.Id;
            response.UserName = user.UserName;
            response.Email = user.Email;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Role = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;

            return response;
        }
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request, string origin)
        {
            RegisterResponse response = new RegisterResponse();

            var existEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existEmail != null)
            {
                response.HasError = true;
                response.Error = $"There is an account with this Email";
                return response;
            }
            var existUserName = await _userManager.FindByNameAsync(request.UserName);
            if (existUserName != null)
            {
                response.HasError = true;
                response.Error = $"This UserName already exists";
                return response;
            }
            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.UserName,
                LastName = request.LastName,
                Cedula = request.Cedula,
                Name = request.Name
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user,request.Role.ToString());
                var verificationUri = await SendVeritionEmailUri(user, origin);
                await _emailService.SendAsync(new Core.Application.Dtos.Email.EmailRequest()
                {
                    To = user.Email,
                    Body = $"<h1>Welcome To Bank Guard, <i>Keep your Money Save</i></h1><br/>" +
                    $"Welcome To Bank Guard, This email is to thank you for joining us. Your account has been created successfully, but we need you" +
                    $"to confirm it in order to activate it. <br/><br/>" +
                    $"By Clicking <a href=\"{verificationUri}\">here</a> you active your account and accept out Terms and Conditions.",
                    Subject = "Confirmation Email."
                });
            }
            else
            {
                response.Error = "An Error occurred by registering this Client";
                response.HasError = true;
                return response;
            }
            return response;

        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return "There is not account for this user";
            }
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return $"The account for {user.Email} has been confirm successfully.";
            }
            else
            {
                return $"An Error occurred whe trying to confirm the {user.Email} account";
            }
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            ForgotPasswordResponse response = new()
            {
                HasError = false
            };
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.Error = $"There is not account registered with {request.Email}";
                response.HasError = true;
                return response;
            }
            var verificationUri = await SendForgotPasswordUri(user, origin);
            await _emailService.SendAsync(new Core.Application.Dtos.Email.EmailRequest
            {
                To = user.Email,
                Subject = "Password Recovery",
                Body = "<h1>Reset Password Request<h1><br/>" +
                $"In order to reset your password <a href=\"{verificationUri}\">Click Here</a>. <br/>" +
                $"if you didn't make this request ignore this Email or contact your Administrator"
            });

            return response;
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ResetPasswordResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.Error = "There isn't an account for this Email address.";
                response.HasError = true;
                return response;
            }
            request.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded)
            {
                response.Error = "An error occurred while reseting the password.";
                response.HasError = true;
                return response;
            }
            return response;
        }


        //SendForgotPassword Email
        private async Task<string> SendForgotPasswordUri(ApplicationUser user, string origin)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ResetPassword";
            var Uri = new Uri(string.Concat($"{origin}/{route}"));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "Token", code);
            return verificationUri;
        }



        //SendConfirmation Email
        private async Task<string> SendVeritionEmailUri(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ConfirmEmail";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "Token", code);

            return verificationUri;
        }

    }
}

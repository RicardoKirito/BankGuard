using AutoMapper;
using BankGuard.Core.Application.Dtos.Account;
using BankGuard.Core.Application.Interfaces.Services;
using BankGuard.Core.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Application.Services
{
    public class AccountService : IAccountServices
    {
        private IMapper _mapper;
        private IAccountService _accountService;


        public AccountService(IMapper mapper, IAccountService account)
        {
            _mapper = mapper;
            _accountService = account;
        }



        public async Task<AuthenticationResponse> LoginAsync(LoginViewModel vm)
        {
            AuthenticationRequest request = _mapper.Map<AuthenticationRequest>(vm);
            AuthenticationResponse response = await _accountService.AuthenticateAsync(request);
            return response;
        }

        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            return await _accountService.ConfirmAccountAsync(userId, token);
        }
        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordViewModel model, string origin)
        {
            ForgotPasswordRequest request = _mapper.Map<ForgotPasswordRequest>(model);
            return await _accountService.ForgotPasswordAsync(request, origin);
        }
        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordViewModel vm)
        {
            ResetPasswordRequest request = _mapper.Map<ResetPasswordRequest>(vm);
            return await _accountService.ResetPasswordAsync(request);
        }
        public async Task SignOut()
        {
            await _accountService.SignOutAsync();
        }
    }
}

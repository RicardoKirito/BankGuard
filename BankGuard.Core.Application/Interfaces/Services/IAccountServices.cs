using BankGuard.Core.Application.Dtos.Account;
using BankGuard.Core.Application.ViewModels.User;

namespace BankGuard.Core.Application.Interfaces.Services
{
    public interface IAccountServices
    {
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordViewModel model, string origin);
        Task<AuthenticationResponse> LoginAsync(LoginViewModel vm);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordViewModel vm);
        Task SignOut();
    }
}
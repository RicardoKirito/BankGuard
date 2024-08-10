using BankGuard.Core.Application.Dtos.Account;
using BankGuard.Core.Application.ViewModels.User;

namespace BankGuard.Core.Application.Interfaces.Services
{
    public interface IAdminServices
    {
        Task<List<UserViewModel>> GetUsersAsync();
        Task<RegisterResponse> RegisterUserAsync(SaveUserViewModel vm, string origin);
        Task<SaveUserViewModel> GetUserById(string id);
        Task<UserUpdateResponse> UpdateAsync(SaveUserViewModel sv);
        Task ChangeSatatus(string id, bool status);
    }
}
using BankGuard.Core.Application.Dtos.Account;
using BankGuard.Core.Application.ViewModels.User;

namespace BankGuard.Infrastructure.Identity.Services
{
    public interface IUserServices
    {
        Task<List<UserRequest>> GetUserListsAsync();
        Task<UserUpdateResponse> UpdateAsync(SaveUserViewModel save);
        Task ChangeStatus(string id, bool status);
        Task<UserRequest> GetUserByid(string id);
    }
}
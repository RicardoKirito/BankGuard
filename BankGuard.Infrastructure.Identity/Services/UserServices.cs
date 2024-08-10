using AutoMapper;
using BankGuard.Core.Application.Dtos.Account;
using BankGuard.Core.Application.Interfaces.Services;
using BankGuard.Core.Application.ViewModels.User;
using BankGuard.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Infrastructure.Identity.Services
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserServices(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<List<UserRequest>> GetUserListsAsync()
        {
            List<UserRequest> userList = new List<UserRequest>();

            var users = _userManager.Users.ToList();

            foreach (var user in users)
            {
                UserRequest request = new UserRequest
                {
                    id = user.Id,
                    Name = user.Name,
                    LastName = user.LastName,
                    Cedula = user.Cedula,
                    Email = user.Email,
                    Role = _userManager.GetRolesAsync(user).Result.ToList()[0],
                    UserName = user.UserName,
                    IsVerified = user.EmailConfirmed
                };
                userList.Add(request);
            }
            return userList;
        }
        public async Task<UserRequest> GetUserByid(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            UserRequest request = new UserRequest
            {
                id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Cedula = user.Cedula,
                Email = user.Email,
                Role = _userManager.GetRolesAsync(user).Result.ToList()[0],
                UserName = user.UserName,
                IsVerified = user.EmailConfirmed
            };
            return request;
        }
        public async Task<UserUpdateResponse> UpdateAsync(SaveUserViewModel save)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(save.id);
            UserUpdateResponse response = new UserUpdateResponse
            {
                HasError = false,
            };
            if(user.UserName != save.UserName)
            {
                var verify = _userManager.FindByNameAsync(save.UserName).Result;
                if (verify != null)
                {
                    response.HasError = true;
                    response.UserNameError = "This UserName Already being used";
                }
            }
            if (user.Email != save.Email)
            {  
                var verify = _userManager.FindByEmailAsync(save.Email).Result;
                if (verify != null)
                {
                    response.HasError = true;
                    response.EmailError = "This Email Already being used";
                }
                if (response.HasError)
                {
                    return response;
                }
            }
            user.Name = save.Name;
            user.LastName = save.LastName;
            user.Cedula = save.Cedula;
            user.UserName = save.UserName;
            user.Email = save.Email;
            var token = _userManager.GeneratePasswordResetTokenAsync(user).Result.ToString();
            await _userManager.ResetPasswordAsync(user, token, save.Password);
            await _userManager.UpdateAsync(user);
            return response;
                                
        }
        public async Task ChangeStatus(string id, bool status)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (status)
            {
                status = false;

            }
            else
            {
                status = true;
            }
            user.EmailConfirmed = status;
            await _userManager.UpdateAsync(user);
        }
    }
}

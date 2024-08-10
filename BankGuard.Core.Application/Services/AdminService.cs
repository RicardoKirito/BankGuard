using AutoMapper;
using BankGuard.Core.Application.Dtos.Account;
using BankGuard.Core.Application.Enums;
using BankGuard.Core.Application.Helpers;
using BankGuard.Core.Application.Interfaces.Repositories;
using BankGuard.Core.Application.Interfaces.Services;
using BankGuard.Core.Application.ViewModels.Product;
using BankGuard.Core.Application.ViewModels.User;
using BankGuard.Core.Domain.Entities;
using BankGuard.Infrastructure.Identity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Application.Services
{
    public class AdminService : IAdminServices
    {
        private readonly IUserServices _userService;
        private readonly IAccountService _accountService;
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;

        private readonly IMapper _mapper;

        public AdminService(IAccountService accountService, IMapper mapper, IProductRepository accountRepository, IUserServices user, IProductService service)
        {
            _accountService = accountService;
            _mapper = mapper;
            _productRepository = accountRepository;
            _userService = user;
            _productService = service;

        }
        public async Task<List<UserViewModel>> GetUsersAsync()
        {
            List<UserRequest> users = await _userService.GetUserListsAsync();
            return _mapper.Map<List<UserViewModel>>(users);
        }
        public async Task<SaveUserViewModel> GetUserById(string id)
        {
            UserViewModel user = GetUsersAsync().Result.FirstOrDefault(u => u.id.Equals(id));
            return _mapper.Map<SaveUserViewModel>(user);
        }
        public async Task<RegisterResponse> RegisterUserAsync(SaveUserViewModel vm, string origin)
        {
            RegisterRequest request = _mapper.Map<RegisterRequest>(vm);
            RegisterResponse response = await _accountService.RegisterAsync(request, origin);
            if (!response.HasError && request.Role==Roles.Basic)
            {

                Product account = new Product
                {
                    
                    amount = null,
                    Balance = vm.InitialAmount,
                    Type = Accounttype.Saving.ToString(),
                    IsPrimary = true,
                    UserId =  _userService.GetUserListsAsync().Result.FirstOrDefault(u => u.UserName.Equals(vm.UserName)).id
                   

                };
                await _productRepository.AddAsync(account);

            }

            return response;
        }
        public async Task<UserUpdateResponse> UpdateAsync(SaveUserViewModel sv)
        {

            UserUpdateResponse response = await _userService.UpdateAsync(sv);
            if (!response.HasError)
            {
                if (sv.InitialAmount > 0)
                {                    
                    ProductViewModel product = await _productService.GetPrimary(sv.id);
                    SaveProductViewModel saveProduct = _mapper.Map<SaveProductViewModel>(product);
                    saveProduct.Balance += sv.InitialAmount;
                    await _productService.Update(saveProduct, product.accountnumber);
                }
            }
            return response;
        }
        public async Task ChangeSatatus(string id, bool status)
        {
            await _userService.ChangeStatus(id, status);
        }


    }
}

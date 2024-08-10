using AutoMapper;
using BankGuard.Core.Application.Dtos.Account;
using BankGuard.Core.Application.ViewModels.Beneficiary;
using BankGuard.Core.Application.ViewModels.Product;
using BankGuard.Core.Application.ViewModels.Transaction;
using BankGuard.Core.Application.ViewModels.User;
using BankGuard.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Application.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region User
            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(x => x.ErrorMessage, opt => opt.Ignore())
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<RegisterRequest, SaveUserViewModel>()
                .ForMember(x => x.InitialAmount, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<ForgotPasswordRequest, ForgotPasswordViewModel>()
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<ResetPasswordRequest, ResetPasswordViewModel>()
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<UserViewModel, UserRequest>()
                .ReverseMap();
            CreateMap<UserViewModel, SaveUserViewModel>()
                .ForMember(x => x.InitialAmount, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.IsVerified, opt => opt.Ignore());
            #endregion

            #region Product
            CreateMap<ProductViewModel,  Product> ()
                .ReverseMap();
            CreateMap<Product, SaveProductViewModel>()
                .ReverseMap();
            CreateMap<ProductViewModel, SaveProductViewModel>()
                .ReverseMap();
            #endregion

            #region Transaction
            CreateMap<Transactions, SaveTransactionViewModel>()
                .ReverseMap(); 


            CreateMap<Transactions, TransactionViewModel>()
                .ReverseMap();

            #endregion
            #region Beneficiary
            CreateMap<Beneficiaries, SaveBeneficiaryViewModel>()
                .ReverseMap ()
                .ForMember(x=> x.Product, opt=> opt.Ignore());
            CreateMap<BeneficiaryViewModel, SaveBeneficiaryViewModel>()
                .ReverseMap()
                .ForMember(x=> x.Product, opt=> opt.Ignore())
                .ForMember(x=> x.LastName, opt=> opt.Ignore())
                .ForMember(x=> x.Name, opt=> opt.Ignore());
            CreateMap<Beneficiaries, BeneficiaryViewModel>()
                .ForMember(x=> x.Name, opt=> opt.Ignore())
                .ForMember(x=> x.LastName, opt=> opt.Ignore())
                .ReverseMap();
            #endregion
        }
    }
}

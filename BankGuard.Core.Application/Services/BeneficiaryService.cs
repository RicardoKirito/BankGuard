using AutoMapper;
using BankGuard.Core.Application.Interfaces.Repositories;
using BankGuard.Core.Application.Interfaces.Services;
using BankGuard.Core.Application.ViewModels.Beneficiary;
using BankGuard.Core.Domain.Entities;
using BankGuard.Infrastructure.Identity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Application.Services
{
    public class BeneficiaryService : GenericService<SaveBeneficiaryViewModel, BeneficiaryViewModel, Beneficiaries, int>, IBeneficiaryService
    {
        private readonly IMapper _mapper;
        private readonly IUserServices _userServices;
        private readonly IBeneficiariesRepository _beneficiariesRepository;
        private readonly IProductService _productService;
        public BeneficiaryService(IGenericRepository<Beneficiaries, int> generic, IMapper mapper, IBeneficiariesRepository beneficiariesRepository, IUserServices userServices, IProductService productService) : base(generic, mapper)
        {
            _mapper = mapper;
            _beneficiariesRepository = beneficiariesRepository;
            _userServices = userServices;
            _productService = productService;
        }
        public async Task<List<BeneficiaryViewModel>> GetAllWithUser()
        {

            var result = await _beneficiariesRepository.GetAllWithInclude(new List<string> { "Product" });

            List<BeneficiaryViewModel> beneficiaries = _mapper.Map<List<BeneficiaryViewModel>>(result);
            foreach (BeneficiaryViewModel beneficiary in beneficiaries)
            {
                beneficiary.Name = _userServices.GetUserByid(beneficiary.Product.UserId).Result.Name;
                beneficiary.LastName = _userServices.GetUserByid(beneficiary.Product.UserId).Result.LastName;
            }
            return beneficiaries;
        }
        public async Task<List<BeneficiaryViewModel>> GetAllWithInclude(List<string> properties)
        {
            var result = await _beneficiariesRepository.GetAllWithInclude(properties);
            return _mapper.Map<List<BeneficiaryViewModel>>(result);
        }

    }
}

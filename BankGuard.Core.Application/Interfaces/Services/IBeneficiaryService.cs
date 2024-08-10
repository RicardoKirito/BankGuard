using BankGuard.Core.Application.ViewModels.Beneficiary;
using BankGuard.Core.Application.ViewModels.Product;
using BankGuard.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Application.Interfaces.Services
{
    public interface IBeneficiaryService : IGenericService<SaveBeneficiaryViewModel, BeneficiaryViewModel, Beneficiaries, int>
    {
        Task<List<BeneficiaryViewModel>> GetAllWithUser();
        Task<List<BeneficiaryViewModel>> GetAllWithInclude(List<string> properties);
    }
}

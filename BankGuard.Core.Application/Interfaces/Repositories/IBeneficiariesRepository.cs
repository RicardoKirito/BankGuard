using BankGuard.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Application.Interfaces.Repositories
{
    public interface IBeneficiariesRepository : IGenericRepository<Beneficiaries, int>
    {
        Task<List<Beneficiaries>> GetAllWithInclude(List<string> properties);
    }
}

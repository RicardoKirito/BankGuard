using BankGuard.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Application.Interfaces.Repositories
{
    public interface ITransactionRepository : IGenericRepository<Transactions, int>
    {
        Task<List<Transactions>> GetByAccountAsync(List<string> accounts);
    }
}


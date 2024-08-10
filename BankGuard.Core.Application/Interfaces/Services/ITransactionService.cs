using BankGuard.Core.Application.Dtos.Transaction;
using BankGuard.Core.Application.ViewModels.Product;
using BankGuard.Core.Application.ViewModels.Transaction;
using BankGuard.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Application.Interfaces.Services
{
    public interface ITransactionService : IGenericService<SaveTransactionViewModel, TransactionViewModel, Transactions, int>
    {
        Task<TransactionResponse> TransactionValitor(SaveTransactionViewModel model);
        Task<List<TransactionViewModel>> GetByAccountAsync(List<string> accounts);
    }
}

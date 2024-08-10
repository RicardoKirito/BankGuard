using BankGuard.Core.Application.Interfaces.Repositories;
using BankGuard.Core.Domain.Entities;
using BankGuard.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository : GenericRepository<Transactions, int>, ITransactionRepository
    {
        public readonly ApplicationContext _context;
        public TransactionRepository(ApplicationContext context): base(context)
        {
            _context = context;
        }
        public async Task<List<Transactions>> GetByAccountAsync(List<string> accounts)
        {
            var transactionlist = _context.Set<Transactions>().ToList();
            List<Transactions> result = new List<Transactions>();
            if(transactionlist.Count() > 0)
            {
                foreach (var transaction in transactionlist)
                {
                    if(accounts.Contains(transaction.AccountFrom))
                    {
                    
                        transaction.Amount *= -1;
                        result.Add(transaction);
                    }
                    if (accounts.Contains(transaction.AccountTo))
                    {
                        result.Add(transaction);
                    }
                }

            }
            return result;
        }
    }
}

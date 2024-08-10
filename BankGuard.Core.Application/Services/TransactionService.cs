using AutoMapper;
using BankGuard.Core.Application.Dtos.Transaction;
using BankGuard.Core.Application.Enums;
using BankGuard.Core.Application.Interfaces.Repositories;
using BankGuard.Core.Application.Interfaces.Services;
using BankGuard.Core.Application.ViewModels.Product;
using BankGuard.Core.Application.ViewModels.Transaction;
using BankGuard.Core.Domain.Entities;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BankGuard.Core.Application.Services
{
    public class TransactionService : GenericService<SaveTransactionViewModel,TransactionViewModel, Transactions, int>, ITransactionService
    {
        private readonly IProductRepository _productRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        public TransactionService(IGenericRepository<Transactions, int> generic, IMapper mapper, IProductRepository productRepository, ITransactionRepository transactionRepository) : base(generic, mapper)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _transactionRepository = transactionRepository;
        }
        public async Task<TransactionResponse> TransactionValitor(SaveTransactionViewModel model)
        {
            TransactionResponse response = new TransactionResponse
            {
                HasError = false,
                Error = new List<string> ()
            };
            var from = await _productRepository.GetById(model.AccountFrom);
            var to = await _productRepository.GetById(model.AccountTo);
            if(model.AccountFrom == model.AccountTo)
            {
                response.HasError = true;
                response.Error.Add("The destination account number is also de receiver");
            }
            if (to == null)
            {
                response.HasError = true;
                response.Error.Add("The destination account number is incorret");
            }
            if (from == null)
            {
                response.HasError = true;
                response.Error.Add("The origin account number is incorret");
            }

            if (from.Balance < model.Amount && from.Type == Accounttype.Saving.ToString())
            {
                response.HasError = true;
                response.Error.Add("This account doesn't have enough funds for this transaction");

            }
            if (to.Type != Accounttype.Saving.ToString())
            {
                if(to.Balance == 0)
                {
                    response.HasError = true;
                    response.Error.Add($"This {to.Type} has been settled");
                }
                if (to.Balance < model.Amount)
                {
                    model.Amount = (decimal)(model.Amount - (model.Amount - to.Balance));
                }


            }
            if(from.Type == Accounttype.CreditCard.ToString())
            {
                if(from.amount < model.Amount+from.Balance)
                {
                    response.HasError = true;
                    response.Error.Add("This amount exceeds the Credit Card limit");
                }
            }

            return response;    
        }
        public override async Task<TransactionViewModel> Add(SaveTransactionViewModel transaction)
        {
            var from = await _productRepository.GetById(transaction.AccountFrom);
            var to = await _productRepository.GetById(transaction.AccountTo);
            
            
            if (to.Type == Accounttype.CreditCard.ToString())
            {
                if (to.amount < transaction.Amount + to.Balance)
                {
                    transaction.Amount = (decimal)(transaction.Amount - (transaction.Amount - to.Balance));
                }

                
            }

            if (transaction.Type == Operations.Payment.ToString())
            {
                if (to.Type != Accounttype.Saving.ToString())
                {
                    from.Balance -= transaction.Amount;
                    to.Balance -= transaction.Amount;
                }
                else
                {
                    from.Balance -= transaction.Amount;
                    to.Balance += transaction.Amount;
                }
            }
            else if (transaction.Type == Operations.Transfer.ToString())
            {
                from.Balance -= transaction.Amount;
                to.Balance += transaction.Amount;
            }
            else if(transaction.Type == Operations.CashAdvance.ToString())
            {
                if(from.Type==Accounttype.CreditCard.ToString())
                {
                    var interest = transaction.Amount * (decimal)0.0625;
                    from.Balance = from.Balance + (transaction.Amount + interest);
                }

            }

            transaction.Date = DateTime.Now.ToString("G");

            await _productRepository.UpdateAsync(from, from.accountnumber);
            await _productRepository.UpdateAsync(to, to.accountnumber);

            Transactions save = _mapper.Map<Transactions>(transaction);
            TransactionViewModel view =_mapper.Map<TransactionViewModel>(await _transactionRepository.AddAsync(save));

           

            return view;

        }

        public async Task<List<TransactionViewModel>> GetByAccountAsync(List<string> accounts)
        {
            var transaction = await _transactionRepository.GetByAccountAsync(accounts);
            return _mapper.Map<List<TransactionViewModel>>(transaction);
        }
    }   
}

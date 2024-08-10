using AutoMapper;
using BankGuard.Core.Application.Dtos.Transaction;
using BankGuard.Core.Application.Enums;
using BankGuard.Core.Application.Interfaces.Repositories;
using BankGuard.Core.Application.Interfaces.Services;
using BankGuard.Core.Application.ViewModels.Product;
using BankGuard.Core.Application.ViewModels.Transaction;
using BankGuard.Core.Domain.Entities;

namespace BankGuard.Core.Application.Services
{
    public class ProductService : GenericService<SaveProductViewModel, ProductViewModel, Product, string>, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        public ProductService(IGenericRepository<Product, string> generic, IMapper mapper, IProductRepository productRepository, ITransactionService transactionService) : base(generic, mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _transactionService = transactionService;
        }
        public override async Task<ProductViewModel> Add(SaveProductViewModel vm)
        {
            var save = _mapper.Map<Product>(vm);

            if(save.Type == Accounttype.Loan.ToString())
            {
                save.Balance = save.amount.Value;
                var primary = await _productRepository.GetPrimary(vm.UserId);
                primary.Balance += vm.amount.Value;
                await _productRepository.UpdateAsync(primary, primary.accountnumber);

            }
            var product = await _productRepository.AddAsync(save);

            return _mapper.Map<ProductViewModel>(product);


        }

        public async Task<ProductViewModel> GetPrimary(string id)
        {
            var product = await _productRepository.GetPrimary(id);
            return _mapper.Map<ProductViewModel>(product); 
        }
        public async Task<List<ProductViewModel>> GetAllByUserid(string id)
        {
            var produt =  await _productRepository.GetAllByUserid(id);
            return _mapper.Map<List<ProductViewModel>>(produt);
        }
        public  async Task<string> DeleteAsync(string id)
        {
            var account = await  _productRepository.GetById(id);
            if(account.Type == Accounttype.Saving.ToString())
            {
                if (account.IsPrimary)
                {
                    return "Primary account cannot be deleted";
                }
                var primary = await GetPrimary(account.UserId);
                SaveTransactionViewModel transaction = new SaveTransactionViewModel
                {
                    AccountFrom = account.accountnumber,
                    AccountTo = primary.accountnumber,
                    By = "SystemDefault",
                    Amount = account.Balance,
                    Type = Operations.Transfer.ToString(),
                    Detail = "Automatic transfer the full account balance to the main account after deleting a Savings account"
                };
                await _transactionService.Add(transaction);
                await _productRepository.DeleteAsync(id);
                return "Delete Succesfully";

            }
            if (account.Type == Accounttype.CreditCard.ToString() || account.Type == Accounttype.Loan.ToString())
            {
                if (account.Balance > 0)
                {
                    return $"Cannot delete this {account.Type} \"{account.accountnumber}\" until it's settled with \"0.00\" balance";
                }
            }
            await _productRepository.DeleteAsync(id);
            return "";
        }
        
        public async Task<List<string>> GetLisAccountNumber(string userid)
        {
            List<string> list = new List<string>();
            var accounts = await _productRepository.GetAllByUserid(userid);
            foreach(Product account in accounts)
            {
                list.Add(account.accountnumber);
            }
            return list;
        }
    }   
}

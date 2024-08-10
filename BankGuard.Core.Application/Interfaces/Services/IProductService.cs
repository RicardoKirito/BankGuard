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
    public interface IProductService : IGenericService<SaveProductViewModel, ProductViewModel, Product, string>
    {
        Task<ProductViewModel> GetPrimary(string id);
        Task<List<ProductViewModel>> GetAllByUserid(string id);
        Task<string> DeleteAsync(string id);
        Task<List<string>> GetLisAccountNumber(string userid);
    }
}

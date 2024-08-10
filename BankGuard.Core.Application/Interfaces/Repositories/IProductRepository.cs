using BankGuard.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Application.Interfaces.Repositories
{
    public interface IProductRepository : IGenericRepository<Product, string>
    {
        Task<List<Product>> GetAllByUserid(string id);
        Task<Product> GetPrimary(string id);
    }
}

using BankGuard.Core.Application.Helpers;
using BankGuard.Core.Application.Interfaces.Repositories;
using BankGuard.Core.Domain.Entities;
using BankGuard.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Infrastructure.Persistence.Repositories
{
    public class ProductRepository: GenericRepository<Product, string>, IProductRepository
    {
        public readonly ApplicationContext _context;
        public ProductRepository(ApplicationContext context): base(context)
        {
            _context = context;
        }

        public override async Task<Product> AddAsync(Product entity)
        {
            var last = _context.Set<Product>().OrderByDescending(a => a.accountnumber).FirstOrDefault();
            if (last == null)
            {
                entity.accountnumber = DateTime.Now.Year.ToString() + "00000";
                return await base.AddAsync(entity);
            }
            int current = int.Parse(last.accountnumber) + 3;
            entity.accountnumber = current.ToString();

            return await base.AddAsync(entity);
        }
        public async Task<List<Product>> GetAllByUserid(string id)
        {
            return _context.Set<Product>().Where(d => d.UserId==id).ToList();
        }
        public async Task<Product> GetPrimary (string id)
        {
           Product product = GetAllByUserid(id).Result.FirstOrDefault(d => d.IsPrimary);
            return product;
        }
    }
}

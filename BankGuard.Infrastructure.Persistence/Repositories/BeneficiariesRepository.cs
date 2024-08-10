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
    public class BeneficiariesRepository : GenericRepository<Beneficiaries, int>, IBeneficiariesRepository
    {
        public readonly ApplicationContext _context;
        public BeneficiariesRepository(ApplicationContext context): base(context)
        {
            _context = context;
        }
        public async Task<List<Beneficiaries>> GetAllWithInclude(List<string> properties)
        {
            var query = _context.Set<Beneficiaries>().AsQueryable();
            foreach (var property in properties)
            {
                query = query.Include(property);
            }
            return query.ToList();
        }

    }
}

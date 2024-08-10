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
    public class LogRepository : GenericRepository<Log, int>, ILogRepository
    {
        public readonly ApplicationContext _context;
        public LogRepository(ApplicationContext context): base(context)
        {
            _context = context;
        }

    }
}

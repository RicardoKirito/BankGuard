using BankGuard.Core.Application.Interfaces.Repositories;
using BankGuard.Infrastructure.Identity.Services;
using BankGuard.Infrastructure.Persistence.Context;
using BankGuard.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDB"))
            {
                services.AddDbContext<ApplicationContext>(p => p.UseInMemoryDatabase("PersistenceDB"));
            }
            else
            {
                services.AddDbContext<ApplicationContext>(option =>
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                m => m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)
                ));
                
            }
            #region Services
            services.AddTransient(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ILogRepository, LogRepository>();
            services.AddTransient<IBeneficiariesRepository, BeneficiariesRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<IUserServices, UserServices>();
            #endregion
        }
    }
}

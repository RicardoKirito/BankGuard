using BankGuard.Core.Application.Interfaces.Services;
using BankGuard.Core.Application.Services;
using BankGuard.Infrastructure.Identity.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddServiceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            #region Services
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient<IAccountServices, AccountService>();
            services.AddTransient<IAdminServices, AdminService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IBeneficiaryService, BeneficiaryService>();
            services.AddTransient(typeof(IGenericService<,,,>), typeof(GenericService<,,,>));
            #endregion
        }
    }
}

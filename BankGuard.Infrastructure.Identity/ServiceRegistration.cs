using BankGuard.Core.Application.Interfaces.Services;
using BankGuard.Infrastructure.Identity.Context;
using BankGuard.Infrastructure.Identity.Entities;
using BankGuard.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Infrastructure.Identity
{
    public static class ServiceRegistration
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("IdentityInMemoryDB"))
            {
                services.AddDbContext<IdentityContext>(p => p.UseInMemoryDatabase("IdentityDB"));
            }
            else
            {

                services.AddDbContext<IdentityContext>(p =>
                {
                    p.EnableSensitiveDataLogging();
                    p.UseSqlServer(configuration.GetConnectionString("IdentityConnection"),
                    m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));

                });
            }

            #region Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(option =>
            {
                option.LoginPath = "/Login/Login";
                option.AccessDeniedPath = "/Login/AccessDenied";
            });
            #endregion

            #region Services
            services.AddTransient<IAccountService, AccountService>();
            #endregion
        }
    }
}

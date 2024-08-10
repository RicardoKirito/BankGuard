using BankGuard.Core.Application.Enums;
using BankGuard.Core.Application.Interfaces.Repositories;
using BankGuard.Core.Domain.Entities;
using BankGuard.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Infrastructure.Identity.Seeds
{
    public class DefaultBasicUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IProductRepository productRepository)
        {
            ApplicationUser basicUser = new();
            basicUser.PhoneNumberConfirmed = true;
            basicUser.EmailConfirmed = true;
            basicUser.Name = "Ricardo";
            basicUser.Cedula = "000-0000000-0";
            basicUser.LastName = "Paniagua";
            basicUser.UserName = "basicUser";
            basicUser.Email = "basicUser@email.com";

            Product defaultsaving = new Product
            {
                Balance = 0,
                IsPrimary = true,
                Type = Accounttype.Saving.ToString()
            };

            if (userManager.Users.All(u=> u.Id != basicUser.Id))
            {
                var user = await userManager.FindByEmailAsync(basicUser.Email);
                if(user == null)
                {
                    await userManager.CreateAsync(basicUser, "Coron@123");
                    await userManager.AddToRoleAsync(basicUser, Roles.Basic.ToString());
                    var product =  productRepository.GetPrimary(basicUser.Id).Result;
                    if(product == null)
                    {
                        defaultsaving.UserId = basicUser.Id;
                        await productRepository.AddAsync(defaultsaving);
                    }
                    
                }
            }
        }
    }
}

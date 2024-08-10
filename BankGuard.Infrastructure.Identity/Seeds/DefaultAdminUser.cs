using BankGuard.Core.Application.Enums;
using BankGuard.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Infrastructure.Identity.Seeds
{
    public class DefaultAdminUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser user = new ApplicationUser();
            user.Name = "Ricardo";
            user.LastName = "Paniagua";
            user.Cedula = "000-0000000-0";
            user.PhoneNumberConfirmed = true;
            user.EmailConfirmed = true;
            user.Email = "AdminUser@email.com";
            user.UserName = "AdminUser";
           
            if(userManager.Users.All(u=> u.Id != user.Id))
            {
                var userAdmin = await userManager.FindByEmailAsync(user.Email);
                if(userAdmin == null)
                {
                    await userManager.CreateAsync(user, "Coron@123");
                    await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                }
            }
        }
    }
}

using BankGuard.Core.Application;
using BankGuard.Core.Application.Interfaces.Repositories;
using BankGuard.Infrastructure.Identity;
using BankGuard.Infrastructure.Identity.Entities;
using BankGuard.Infrastructure.Identity.Seeds;
using BankGuard.Infrastructure.Persistence;
using BankGuard.Infrastructure.Shared;
using BankGuard.Middleware;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddServiceLayer(builder.Configuration);
builder.Services.AddSharedInterface(builder.Configuration);
builder.Services.AddSingleton<ValidateSession, ValidateSession>();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddScoped<LoginAuthorize>();
builder.Services.AddScoped<AdminAuthorize>();
builder.Services.AddIdentityInfrastructure(builder.Configuration);
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var ProductRepository = services.GetRequiredService<IProductRepository>();

        await DefaultRoles.SeedAsync(userManager, roleManager);
        await DefaultBasicUser.SeedAsync(userManager, roleManager, ProductRepository);
        await DefaultAdminUser.SeedAsync(userManager, roleManager);
    }
    catch(Exception ex)
    {

    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();

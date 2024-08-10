using BankGuard.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BankGuard.Middleware
{
    public class AdminAuthorize : IAsyncActionFilter
    {
        private readonly ValidateSession _session;
        public AdminAuthorize(ValidateSession session)
        {
            _session = session;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_session.HasUser())
            {
                if (_session.GetUserRole() == "Admin")
                {
                    var controller = (AdminController)context.Controller;
                    context.Result = controller.RedirectToAction("Home", "Admin");

                }
                else
                {
                    var controller = (AdminController)context.Controller;
                    context.Result = controller.RedirectToAction("Index", "Basic");
                }
            }
            else
            {
                await next();
            }
        }
    }
}

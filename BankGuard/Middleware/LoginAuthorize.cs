using BankGuard.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BankGuard.Middleware
{
    public class LoginAuthorize : IAsyncActionFilter
    {
        private readonly ValidateSession _session;
        public LoginAuthorize(ValidateSession session)
        {
            _session = session;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_session.HasUser())
            {
                if (_session.GetUserRole() == "Admin")
                {
                    var controller = (UserController)context.Controller;
                    context.Result = controller.RedirectToAction("Home", "Admin");

                }
                else
                {
                    var controller = (UserController)context.Controller;
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

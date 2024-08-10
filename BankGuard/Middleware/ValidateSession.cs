using BankGuard.Core.Application.Dtos.Account;
using BankGuard.Core.Application.Enums;
using BankGuard.Core.Application.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BankGuard.Middleware
{
    public class ValidateSession 
    {
        private IHttpContextAccessor _contextAccessor;
        public ValidateSession(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public bool HasUser()
        {
            AuthenticationResponse user =_contextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            if(user == null)
            {
                return false;
            }
            return true;
        }
        public string GetUserRole()
        {
            AuthenticationResponse user = _contextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            if(user != null)
            {
                return user.Role[0].ToString();
            }

            return null;
        }
    }
}

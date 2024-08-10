using BankGuard.Core.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Application.Dtos.Account
{
    public class UserUpdateResponse
    {
        public bool HasError { get; set; }
        public string UserNameError { get; set; }
        public string EmailError { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Application.ViewModels.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="UserName or email Is required")]
        [DataType(DataType.Text)]
        public string Email { get; set; }
        [Required(ErrorMessage="The password is Required")]
        [DataType (DataType.Password)]
        public string Password { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
    }
}

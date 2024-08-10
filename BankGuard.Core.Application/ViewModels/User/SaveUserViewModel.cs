using BankGuard.Core.Application.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Application.ViewModels.User
{
    public class SaveUserViewModel
    {
        public string id { get; set; }
        [Required(ErrorMessage = "This field required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This field required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "This field required")]
        public string Cedula { get; set; }
        [Required(ErrorMessage = "This field required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "This field required")]        
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "The password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "The password is Required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The password doesn't match")]
        public string ConfirmPassword { get; set; }
        [Required]
        public Roles Role { get; set; }
        [Range(0,int.MaxValue)]
        public int InitialAmount { get; set; }
        public bool HasError { get; set; }
        public string Error { get; set; }
    }
}

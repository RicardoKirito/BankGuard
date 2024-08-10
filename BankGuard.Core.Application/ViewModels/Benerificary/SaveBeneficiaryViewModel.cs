using BankGuard.Core.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Application.ViewModels.Beneficiary
{
    public class SaveBeneficiaryViewModel
    {
        public int? Id { get; set; }

        public string? Userid { get; set; }
        [MinLength(9, ErrorMessage = "The account needs to have 9 digits")]
        [MaxLength(9, ErrorMessage = "The account needs to have 9 digits")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage ="Need account number to add a new Beneficiary")]
        public string Accountnumber { get; set; }

    }
}

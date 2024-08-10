using BankGuard.Core.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Application.ViewModels.Beneficiary
{
    public class BeneficiaryViewModel
    {
        public int Id { get; set; }
        public string Userid { get; set; }
        public string Accountnumber { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }


        public ProductViewModel Product { get; set; }

    }
}

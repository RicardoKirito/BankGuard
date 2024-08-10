using BankGuard.Core.Application.Dtos.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Application.ViewModels.Product
{
    public class ProductViewModel
    {
        public string accountnumber { get; set; }
        public decimal Balance { get; set; }
        public decimal? amount { get; set; }
        public string Type { get; set; }
        public bool IsPrimary { get; set; }
        public string UserId { get; set; }

    }
}

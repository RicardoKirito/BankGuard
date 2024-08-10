using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Domain.Entities
{
    public class Product
    {
        public string accountnumber { get; set; }
        public decimal Balance { get; set; }
        public decimal? amount { get; set; }
        public string Type { get; set; }
        public bool IsPrimary { get; set; }
        public string UserId { get; set; }

    }
}

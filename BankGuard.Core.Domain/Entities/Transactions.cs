using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Domain.Entities
{
    public class Transactions
    {
        public int Id { get; set; }
        public string AccountFrom { get; set; }
        public string AccountTo { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string By { get; set; }
        public string Date { get; set; }
        public string? Detail { get; set; }



    }
}

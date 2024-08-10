using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Domain.Entities
{
    public class Beneficiaries
    {
        public int Id { get; set; }
        public string Userid { get; set; }
        public string Accountnumber { get; set; }  
        public string? Alias  { get; set; }

        public Product Product { get; set; }

    }
}

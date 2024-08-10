using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Domain.Entities
{
    public class Log
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string By { get; set; }
        public string To { get; set; }
        public string Date { get; set; }

    }
}

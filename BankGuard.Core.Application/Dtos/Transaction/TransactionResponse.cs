using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Application.Dtos.Transaction
{
    public class TransactionResponse
    {
        public List<string> Error { get; set; }
        public bool HasError { get; set; }
    }
}

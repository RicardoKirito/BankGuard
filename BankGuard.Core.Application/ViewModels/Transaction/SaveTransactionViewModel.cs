using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Application.ViewModels.Transaction
{
    public class SaveTransactionViewModel
    {
        public int? Id { get; set; }
        [Required]
        [MinLength(9, ErrorMessage ="The account should have 9 digits.")]
        [MaxLength(9, ErrorMessage = "The account should have 9 digits.")]
        public string AccountFrom { get; set; }
        [Required]
        [MinLength(9, ErrorMessage = "The account should have 9 digits.")]
        [MaxLength(9, ErrorMessage = "The account should have 9 digits.")]
        public string AccountTo { get; set; }
        [Range(1, int.MaxValue, ErrorMessage ="Amount should be more than $1.00")]
        public decimal Amount { get; set; }
        public string? Type { get; set; }
        public string? By { get; set; }
        public string? Date { get; set; }
        public string? Detail { get; set; }

    }
}

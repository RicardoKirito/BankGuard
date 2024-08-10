using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Application.ViewModels.Product
{
    public class SaveProductViewModel
    {
        [AllowNull]
        [MaybeNull]
        public string? accountnumber { get; set; }
            
        [AllowNull]
        public decimal? Balance { get; set; }
        [Range(5000, int.MaxValue, ErrorMessage ="The minimun amount is 5,000.00")]
        [AllowNull]
        public decimal? amount { get; set; }
        [Required(ErrorMessage ="This field is required")]
        public string Type { get; set; }
        public bool? IsPrimary { get; set; }
        public string UserId { get; set; }

    }
}

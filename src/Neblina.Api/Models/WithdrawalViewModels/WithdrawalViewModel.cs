using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Models.WithdrawalViewModels
{
    public class WithdrawalViewModel
    {
        [Range(0.01, 1000000)]
        public decimal Amount { get; set; }
    }
}

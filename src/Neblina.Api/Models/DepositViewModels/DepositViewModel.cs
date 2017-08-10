using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Models.DepositViewModels
{
    public class DepositViewModel
    {
        [Range(0, 1000000)]
        public decimal Amount { get; set; }
    }
}

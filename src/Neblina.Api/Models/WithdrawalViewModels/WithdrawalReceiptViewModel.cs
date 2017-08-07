using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Models.WithdrawalViewModels
{
    public class WithdrawalReceiptViewModel
    {
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Models.WithdrawalViewModels
{
    public class WithdrawalReceiptViewModel
    {
        private decimal _amount;

        public int TransactionId { get; set; }
        public decimal Amount { get { return _amount; } set { _amount = Math.Abs(value); } }
    }
}

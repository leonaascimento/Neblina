using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Core.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int SourceBankId { get; set; }
        public int SourceAccountId { get; set; }
        public int DestinationBankId { get; set; }
        public int DestinationAccountId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }
        
        public int AccountId { get; set; }
        public Account Account { get; set; }        
    }
}

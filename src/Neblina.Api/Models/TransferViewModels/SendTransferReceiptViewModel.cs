using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Models.TransferViewModels
{
    public class SendTransferReceiptViewModel
    {
        public int TransactionId { get; set; }
        public int DestinationBankId { get; set; }
        public int DestinationAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}

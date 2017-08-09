using Neblina.Api.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Models.TransferViewModels
{
    public class ReceiveTransferViewModel
    {
        public int SourceBankId { get; set; }
        public int SourceAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}

using Neblina.Api.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Models.TransferViewModels
{
    public class ReceiveTransferViewModel
    {
        public int SourceBankId { get; set; }
        public int SourceAccountId { get; set; }
        public int DestinationBankId { get; set; }
        public int DestinationAccountId { get; set; }
        [Range(0.01, 1000000)]
        public decimal Amount { get; set; }
    }
}

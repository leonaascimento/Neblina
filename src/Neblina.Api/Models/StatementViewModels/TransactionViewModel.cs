using Neblina.Api.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Models.StatementViewModels
{
    public class TransactionViewModel
    {
        public int TransactionId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Debit { get; set; }
        public TransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }
    }
}

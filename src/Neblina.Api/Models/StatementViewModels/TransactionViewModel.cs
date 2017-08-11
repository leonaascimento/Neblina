using Neblina.Api.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
        [JsonConverter(typeof(StringEnumConverter))]
        public TransactionType Type { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TransactionStatus Status { get; set; }
    }
}

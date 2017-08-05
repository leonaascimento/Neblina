using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public decimal Total { get; set; }
        public bool Enabled { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public List<Transaction> Transactions { get; set; }
    }
}

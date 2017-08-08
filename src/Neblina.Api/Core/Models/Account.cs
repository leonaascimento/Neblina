using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Core.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public decimal Balance { get; set; }
        public bool Enabled { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public List<Transaction> Transactions { get; set; }
    }
}

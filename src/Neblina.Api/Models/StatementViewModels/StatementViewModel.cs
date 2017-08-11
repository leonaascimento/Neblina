using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Models.StatementViewModels
{
    public class StatementViewModel
    {
        public StatementViewModel()
        {
            Transactions = new List<TransactionViewModel>();
        }

        public int AccountId { get; set; }
        public string CustomerName { get; set; }
        public decimal Balance { get; set; }

        public List<TransactionViewModel> Transactions { get; set; }
    }
}

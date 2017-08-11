using Neblina.Api.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neblina.Api.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Neblina.Api.Persistence.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        protected BankingContext BankingContext { get { return _context as BankingContext; } }

        public TransactionRepository(BankingContext context)
            : base(context)
        {
        }

        public IEnumerable<Transaction> GetAll(int accountId)
        {
            return BankingContext.Transactions.Where(p => p.AccountId == accountId);
        }
    }
}

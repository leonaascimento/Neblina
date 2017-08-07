using Neblina.Api.Core;
using Neblina.Api.Core.Repositories;
using Neblina.Api.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BankingContext _context;

        private IAccountRepository _accounts;
        private ITransactionRepository _transactions;

        public IAccountRepository Accounts => _accounts ?? (_accounts = new AccountRepository(_context));
        public ITransactionRepository Transactions => _transactions ?? (_transactions = new TransactionRepository(_context));

        public UnitOfWork(BankingContext context)
        {
            _context = context;
        }

        public void SaveAndApply()
        {
            _context.SaveChanges();
        }
    }
}

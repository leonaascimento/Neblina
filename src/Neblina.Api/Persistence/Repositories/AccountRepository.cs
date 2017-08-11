using Microsoft.EntityFrameworkCore;
using Neblina.Api.Core.Models;
using Neblina.Api.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Persistence.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        protected BankingContext BankingContext { get { return _context as BankingContext; } }

        public AccountRepository(BankingContext context)
            : base(context)
        {
        }

        public Account GetAccountWithCustomerAndTransactions(int id)
        {
            var account = BankingContext.Accounts
                .Include(p => p.Customer)
                .Where(p => p.AccountId == id)
                .AsNoTracking()
                .SingleOrDefault();

            if (account != null)
                account.Transactions = BankingContext.Transactions
                    .OrderByDescending(p => p.Date)
                    .AsNoTracking()
                    .ToList();

            return account;
        }

        public Account GetAccountWithCustomerAndTransactionsFromDate(int id, DateTime start)
        {
            var account = BankingContext.Accounts
                .Include(p => p.Customer)
                .Where(p => p.AccountId == id)
                .AsNoTracking()
                .SingleOrDefault();

            if (account != null)
                account.Transactions = BankingContext.Transactions
                    .Where(p => p.Date >= start)
                    .OrderByDescending(p => p.Date)
                    .AsNoTracking()
                    .ToList();

            return account;
        }

        public override void Remove(Account entity)
        {
            BankingContext.Entry(entity).State = EntityState.Modified;
            entity.Enabled = false;
        }

        public override void RemoveRange(IEnumerable<Account> entities)
        {
            foreach (var entity in entities)
                Remove(entity);
        }
    }
}

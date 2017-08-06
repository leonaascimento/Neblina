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

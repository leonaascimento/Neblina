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

        public IAccountRepository Accounts { get; private set; }

        public UnitOfWork(BankingContext context)
        {
            _context = context;
            Accounts = new AccountRepository(context);
        }

        public void SaveAndApply()
        {
            _context.SaveChanges();
        }
    }
}

using Neblina.Binder.Api.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neblina.Binder.Api.Core.Repositories;
using Neblina.Binder.Api.Persistence.Repositories;

namespace Neblina.Binder.Api.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BinderContext _context;
        private IBankRepository _banks;

        public IBankRepository Banks => _banks ?? (_banks = new BankRepository(_context));

        public UnitOfWork(BinderContext context)
        {
            _context = context;
        }

        public void SaveAndApply()
        {
            _context.SaveChanges();
        }
    }
}

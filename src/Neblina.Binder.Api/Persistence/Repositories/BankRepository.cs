using Neblina.Binder.Api.Core.Repositories;
using Neblina.Binder.Api.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Neblina.Binder.Api.Persistence.Repositories
{
    public class BankRepository : Repository<Bank>, IBankRepository
    {
        public BankRepository(DbContext context)
            : base(context)
        {
        }
    }
}

using Neblina.Api.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Core.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Account GetAccountWithCustomerAndTransactions(int id);
        Account GetAccountWithCustomerAndTransactionsFromDate(int id, DateTime start);
    }
}

using Neblina.Api.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Core
{
    public interface IUnitOfWork
    {
        IAccountRepository Accounts { get; }
        ITransactionRepository Transactions { get; }
        void SaveAndApply();
    }
}

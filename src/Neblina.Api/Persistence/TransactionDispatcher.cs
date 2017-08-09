using Microsoft.EntityFrameworkCore;
using Neblina.Api.Core;
using Neblina.Api.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Persistence
{
    public class TransactionDispatcher : ITransactionDispatcher
    {
        private readonly BankingContext _context;
        private Queue<int> queue;

        public TransactionDispatcher(BankingContext context)
        {
            _context = context;

            queue = new Queue<int>();
        }

        public void Enqueue(int id)
        {
            queue.Enqueue(id);
        }

        public void Execute(int id)
        {
            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
            {
                try
                {
                    var t = _context.Transactions.Find(id);
                    var a = _context.Accounts.Find(t.AccountId);

                    if (t.Amount > 0 && t.Status == TransactionStatus.Authorized)
                    {
                        a.Balance += t.Amount;
                        t.Status = TransactionStatus.Successful;
                        t.Date = DateTime.Now;
                    }
                    else if (t.Amount < 0 && t.Status == TransactionStatus.Pending)
                    {
                        if (a.Balance >= Math.Abs(t.Amount))
                        {
                            a.Balance += t.Amount;
                            t.Status = TransactionStatus.Authorized;
                            t.Date = DateTime.Now;
                        }
                        else
                        {
                            t.Status = TransactionStatus.Denied;
                            t.Date = DateTime.Now;
                        }
                    }

                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    throw;
                }
            }
        }

        private void Load()
        {
            var ids = _context.Transactions
                .Where(p => p.Status < TransactionStatus.Successful)
                .OrderBy(p => p.Date)
                .AsNoTracking()
                .Select(p => p.TransactionId)
                .ToList();

            foreach (var id in ids)
                queue.Enqueue(id);
        }
    }
}

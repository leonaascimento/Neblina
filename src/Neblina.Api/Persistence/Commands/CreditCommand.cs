using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Neblina.Api.Core.Models;
using System.Threading;
using Neblina.Api.Core.Commands;

namespace Neblina.Api.Persistence.Commands
{
    public class CreditCommand : ICreditCommand
    {
        private readonly BankingContext _context;

        public CreditCommand(BankingContext context)
        {
            _context = context;
        }

        public bool Execute(int id, int tries = 3, int waitInterval = 100)
        {
            var next = false;
            var succeeded = false;

            do
            {
                using (var contextTransaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        var transaction = _context.Transactions.Find(id);

                        if (transaction == null)
                            throw new ArgumentException();

                        var account = _context.Accounts.Find(transaction.AccountId);
                        account.Balance += transaction.Amount;
                        transaction.Status = TransactionStatus.Successful;
                        next = true;

                        _context.SaveChanges();
                        contextTransaction.Commit();

                        succeeded = true;
                    }
                    catch
                    {
                        if (--tries <= 0)
                            throw;
                        else
                            Thread.Sleep(waitInterval);
                    }
                }
            } while (!succeeded);

            return next;
        }
    }
}

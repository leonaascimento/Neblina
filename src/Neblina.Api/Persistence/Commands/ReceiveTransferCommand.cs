using Microsoft.EntityFrameworkCore;
using Neblina.Api.Core.Commands;
using Neblina.Api.Core.Models;
using Neblina.Api.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Neblina.Api.Persistence.Commands
{
    public class ReceiveTransferCommand : IReceiveTransferCommand
    {
        private readonly BankingContext _context;

        public ReceiveTransferCommand(BankingContext context)
        {
            _context = context;
        }

        public void Execute(int id, int tries = 3, int waitInterval = 100)
        {
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
        }
    }
}

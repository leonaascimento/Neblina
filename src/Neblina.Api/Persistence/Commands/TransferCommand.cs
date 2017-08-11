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
    public class TransferCommand : ITransferCommand
    {
        private readonly BankingContext _context;

        public TransferCommand(BankingContext context)
        {
            _context = context;
        }

        public bool Continue(int id, int tries = 3, int waitInterval = 100)
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

                        if (account.Balance >= Math.Abs(transaction.Amount))
                        {
                            account.Balance += transaction.Amount;
                            transaction.Status = TransactionStatus.Authorized;
                            next = true;
                        }
                        else
                            transaction.Status = TransactionStatus.Denied;

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

        public bool Rollback(int id, int tries = 3, int waitInterval = 100)
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

                        account.Balance -= transaction.Amount;
                        transaction.Status = TransactionStatus.Failed;
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

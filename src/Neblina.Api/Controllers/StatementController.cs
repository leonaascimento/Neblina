using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neblina.Api.Core;
using Neblina.Api.Models.StatementViewModels;
using Neblina.Api.Core.Models;

namespace Neblina.Api.Controllers
{
    [Route("statement")]
    public class StatementController : Controller
    {
        private IUnitOfWork _repos;
        private int _accountId;

        public StatementController(IUnitOfWork repos)
        {
            _repos = repos;
            _accountId = 1;
        }

        // GET: statement
        [HttpGet]
        public IActionResult Get(DateTime? from)
        {
            Account account;

            if (from.HasValue)
                account = _repos.Accounts.GetAccountWithCustomerAndTransactionsFromDate(_accountId, from.Value);
            else
                account = _repos.Accounts.GetAccountWithCustomerAndTransactions(_accountId);

            var statement = new StatementViewModel()
            {
                AccountId = account.AccountId,
                CustomerName = account.Customer.Name,
                Balance = account.Balance
            };

            foreach (var transaction in account.Transactions)
                statement.Transactions.Add(new TransactionViewModel()
                {
                    TransactionId = transaction.TransactionId,
                    Date = transaction.Date,
                    Description = transaction.Description,
                    Credit = transaction.Amount > 0 ? transaction.Amount as decimal? : null,
                    Debit = transaction.Amount < 0 ? transaction.Amount as decimal? : null,
                    Type = transaction.Type,
                    Status = transaction.Status
                });

            return Ok(statement);
        }
    }
}

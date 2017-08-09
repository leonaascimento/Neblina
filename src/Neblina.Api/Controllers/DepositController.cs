using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neblina.Api.Models.DepositViewModels;
using Neblina.Api.Core.Models;
using Neblina.Api.Core;
using Neblina.Api.Core.Commands;

namespace Neblina.Api.Controllers
{
    [Route("deposits")]
    public class DepositController : Controller
    {
        private readonly IUnitOfWork _repos;
        private readonly IDepositCommand _command;
        private int _accountId;

        public DepositController(IUnitOfWork repos, IDepositCommand command)
        {
            _repos = repos;
            _command = command;
            _accountId = 1;
        }

        // POST deposits
        [HttpPost]
        public IActionResult Post([FromBody]DepositViewModel deposit)
        {
            var transaction = new Transaction()
            {
                AccountId = _accountId,
                Date = DateTime.Now,
                Description = "Deposit",
                Amount = deposit.Amount,
                Type = TransactionType.SameAccount,
                Status = TransactionStatus.Authorized,
            };

            _repos.Transactions.Add(transaction);
            _repos.SaveAndApply();

            _command.Enqueue(transaction.TransactionId);

            var receipt = new DepositReceiptViewModel()
            {
                TransactionId = transaction.TransactionId,
                Amount = transaction.Amount
            };

            return Ok(receipt);
        }
    }
}

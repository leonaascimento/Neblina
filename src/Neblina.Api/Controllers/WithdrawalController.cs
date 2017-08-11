using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neblina.Api.Models.WithdrawalViewModels;
using Neblina.Api.Core.Models;
using Neblina.Api.Core;
using Neblina.Api.Core.Commands;
using Microsoft.Extensions.Logging;

namespace Neblina.Api.Controllers
{
    [Route("withdrawals")]
    public class WithdrawalController : Controller
    {
        private readonly IUnitOfWork _repos;
        private readonly IDebitCommand _command;
        private int _accountId;

        private ILogger _logger;

        public WithdrawalController(IUnitOfWork repos, IDebitCommand command, ILogger<WithdrawalController> logger)
        {
            _repos = repos;
            _command = command;
            _accountId = 1;
            _logger = logger;
        }

        // POST withdrawals
        [HttpPost]
        public IActionResult Post([FromBody]WithdrawalViewModel withdrawal)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var transaction = new Transaction()
            {
                Date = DateTime.Now,
                Description = "Withdrawal",
                AccountId = _accountId,
                Amount = withdrawal.Amount * -1,
                Type = TransactionType.SameAccount,
                Status = TransactionStatus.Pending
            };

            _repos.Transactions.Add(transaction);
            _repos.SaveAndApply();

            _command.Execute(transaction.TransactionId);

            var receipt = new WithdrawalReceiptViewModel()
            {
                TransactionId = transaction.TransactionId,
                Amount = transaction.Amount
            };

            var processed = _repos.Transactions.Get(transaction.TransactionId);
            if (processed.Status != TransactionStatus.Successful)
                return BadRequest(receipt);    

            _logger.LogInformation($"Someone asked for a withdrawal");        

            return Ok(receipt);
        }
    }
}

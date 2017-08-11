using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neblina.Api.Models.DepositViewModels;
using Neblina.Api.Core.Models;
using Neblina.Api.Core;
using Neblina.Api.Core.Commands;
using Neblina.Api.Core.Dispatchers;
using Microsoft.Extensions.Logging;

namespace Neblina.Api.Controllers
{
    [Route("deposits")]
    public class DepositController : Controller
    {
        private readonly IUnitOfWork _repos;
        private readonly IDepositDispatcher _dispatcher;
        private int _accountId;

        private ILogger _logger;

        public DepositController(IUnitOfWork repos, IDepositDispatcher dispatcher, ILogger<DepositController> logger)
        {
            _repos = repos;
            _dispatcher = dispatcher;
            _accountId = 1;
            _logger = logger;
        }

        // POST deposits
        [HttpPost]
        public IActionResult Post([FromBody]DepositViewModel deposit)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var transaction = new Transaction()
            {
                AccountId = _accountId,
                Date = DateTime.Now,
                Description = "Deposit",
                Amount = deposit.Amount,
                Type = TransactionType.SameAccount,
                Status = TransactionStatus.Authorized,
            };
            
            _logger.LogInformation($"Someone asked for a deposit");

            _repos.Transactions.Add(transaction);
            _repos.SaveAndApply();

            _dispatcher.Enqueue(transaction.TransactionId);

            var receipt = new DepositReceiptViewModel()
            {
                TransactionId = transaction.TransactionId,
                Amount = transaction.Amount
            };

            return Ok(receipt);
        }
    }
}

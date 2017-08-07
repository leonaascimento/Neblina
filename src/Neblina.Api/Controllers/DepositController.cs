using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neblina.Api.Models.DepositViewModels;
using Neblina.Api.Core.Models;
using Neblina.Api.Core;

namespace Neblina.Api.Controllers
{
    [Route("deposits")]
    public class DepositController : Controller
    {
        private readonly IUnitOfWork _repos;
        private int _accountId;

        public DepositController(IUnitOfWork repos)
        {
            _repos = repos;
            _accountId = 1;
        }

        // POST deposits
        [HttpPost]
        public IActionResult Post([FromBody]DepositViewModel deposit)
        {
            var transaction = new Transaction()
            {
                AccountId = _accountId,
                Amount = deposit.Amount,
                Type = TransactionType.SameAccount,
                Status = TransactionStatus.Pending
            };

            _repos.Transactions.Add(transaction);
            _repos.SaveAndApply();

            var receipt = new DepositReceiptViewModel()
            {
                TransactionId = transaction.TransactionId,
                Amount = transaction.Amount
            };

            return Ok(receipt);
        }
    }
}

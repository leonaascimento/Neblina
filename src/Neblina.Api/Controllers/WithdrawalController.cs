using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neblina.Api.Models.WithdrawalViewModels;
using Neblina.Api.Core.Models;
using Neblina.Api.Core;

namespace Neblina.Api.Controllers
{
    [Route("withdrawals")]
    public class WithdrawalController : Controller
    {
        private readonly IUnitOfWork _repos;
        private int _accountId;

        public WithdrawalController(IUnitOfWork repos)
        {
            _repos = repos;
            _accountId = 1;
        }

        // POST withdrawals
        [HttpPost]
        public IActionResult Post([FromBody]WithdrawalViewModel withdrawal)
        {
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

            var receipt = new WithdrawalReceiptViewModel()
            {
                TransactionId = transaction.TransactionId,
                Amount = transaction.Amount
            };

            return Ok(receipt);
        }
    }
}

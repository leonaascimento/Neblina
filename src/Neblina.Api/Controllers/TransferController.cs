using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neblina.Api.Core;
using Neblina.Api.Models.TransferViewModels;
using Neblina.Api.Core.Models;

namespace Neblina.Api.Controllers
{
    [Route("transfers")]
    public class TransferController : Controller
    {
        private IUnitOfWork _repos;
        private int _accountId;

        public TransferController(IUnitOfWork repos)
        {
            _repos = repos;
            _accountId = 1;
        }

        // POST transfers/send
        [HttpPost("send")]
        public IActionResult Send(string bank, bool scheduled, [FromBody]SendTransferViewModel transfer)
        {
            TransactionType type;

            switch (bank)
            {
                case "same":
                    type = scheduled ? TransactionType.SameBankScheduled : TransactionType.SameBankRealTime;
                    break;
                case "another":
                    type = scheduled ? TransactionType.AnotherBankScheduled : TransactionType.AnotherBankRealTime;
                    break;
                default:
                    return NotFound();
            }

            var transaction = new Transaction()
            {
                Date = DateTime.Now,
                Description = "Transfer sent",
                AccountId = _accountId,
                DestinationBankId = transfer.DestinationBankId,
                DestinationAccountId = transfer.DestinationAccountId,
                Amount = transfer.Amount * -1,
                Type = type,
                Status = TransactionStatus.Pending
            };

            _repos.Transactions.Add(transaction);
            _repos.SaveAndApply();

            var receipt = new SendTransferReceiptViewModel()
            {
                TransactionId = transaction.TransactionId,
                DestinationBankId = transfer.DestinationBankId,
                DestinationAccountId = transfer.DestinationAccountId,
                Amount = transaction.Amount
            };

            return Ok(receipt);
        }
    }
}

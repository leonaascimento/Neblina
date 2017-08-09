using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neblina.Api.Core;
using Neblina.Api.Models.TransferViewModels;
using Neblina.Api.Core.Models;
using Neblina.Api.Core.Commands;

namespace Neblina.Api.Controllers
{
    [Route("transfers")]
    public class TransferController : Controller
    {
        private IUnitOfWork _repos;
        private ISendTransferCommand _sendCommand;
        private IReceiveTransferCommand _receiveCommand;
        private int _accountId;

        public TransferController(IUnitOfWork repos, ISendTransferCommand sendCommand, IReceiveTransferCommand receiveCommand)
        {
            _repos = repos;
            _sendCommand = sendCommand;
            _receiveCommand = receiveCommand;
            _accountId = 1;
        }

        // POST transfers/send
        [HttpPost("send")]
        public IActionResult Send(string bank, bool scheduled, [FromBody]SendTransferViewModel transfer)
        {
            TransactionType type;

            if (scheduled)
                return BadRequest(new { message = "Only real-time transactions are supported at the moment." });

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

            _sendCommand.Enqueue(transaction.TransactionId);

            var receipt = new SendTransferReceiptViewModel()
            {
                TransactionId = transaction.TransactionId,
                DestinationBankId = transfer.DestinationBankId,
                DestinationAccountId = transfer.DestinationAccountId,
                Amount = transaction.Amount
            };

            return Ok(receipt);
        }

        // POST transfers/receive
        [HttpPost("receive")]
        public IActionResult Receive(string bank, bool scheduled, [FromBody]ReceiveTransferViewModel transfer)
        {
            TransactionType type;

            if (scheduled)
                return BadRequest(new { message = "Only real-time transactions are supported at the moment." });

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
                Description = "Transfer received",
                AccountId = _accountId,
                DestinationBankId = transfer.SourceBankId,
                DestinationAccountId = transfer.SourceAccountId,
                Amount = transfer.Amount * -1,
                Type = type,
                Status = TransactionStatus.Pending
            };

            _repos.Transactions.Add(transaction);
            _repos.SaveAndApply();

            _receiveCommand.Execute(transaction.TransactionId);

            var receipt = new ReceiveTransferReceiptViewModel()
            {
                TransactionId = transaction.TransactionId,
                SourceBankId = transfer.SourceBankId,
                SourceAccountId = transfer.SourceAccountId,
                Amount = transaction.Amount
            };

            return Ok(receipt);
        }
    }
}

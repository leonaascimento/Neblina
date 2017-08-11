using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neblina.Api.Core;
using Neblina.Api.Models.TransferViewModels;
using Neblina.Api.Core.Models;
using Neblina.Api.Core.Commands;
using Neblina.Api.Core.Dispatchers;
using Neblina.Api.Communicators;
using Microsoft.Extensions.Logging;

namespace Neblina.Api.Controllers
{
    [Route("transfers")]
    public class TransferController : Controller
    {
        private readonly IUnitOfWork _repos;
        private readonly ITransferDispatcher _dispatcher;
        private readonly ICreditCommand _command;
        private readonly RegisterBank _registration;
        private ILogger _logger;
        private int _accountId;

        public TransferController(IUnitOfWork repos, ITransferDispatcher dispatcher, ICreditCommand command, RegisterBank registration, ILogger logger)
        {
            _repos = repos;
            _dispatcher = dispatcher;
            _command = command;
            _registration = registration;
            _logger = logger;
            _accountId = 1;
        }

        // POST transfers/send
        [HttpPost("send")]
        public IActionResult Send(string bank, [FromBody]SendTransferViewModel transfer)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var type = GetType(bank);

            if (!type.HasValue)
                return NotFound();

            var transaction = new Transaction()
            {
                Date = DateTime.Now,
                Description = "Transfer sent",
                AccountId = _accountId,
                SourceBankId = _registration.BankId,
                SourceAccountId = _accountId,
                DestinationBankId = type == TransactionType.SameAccount ? 0 : transfer.DestinationBankId,
                DestinationAccountId = transfer.DestinationAccountId,
                Amount = transfer.Amount * -1,
                Type = type.Value,
                Status = TransactionStatus.Pending
            };

            _repos.Transactions.Add(transaction);
            _repos.SaveAndApply();

            _dispatcher.Enqueue(transaction.TransactionId);

            var receipt = new SendTransferReceiptViewModel()
            {
                TransactionId = transaction.TransactionId,
                DestinationBankId = transfer.DestinationBankId,
                DestinationAccountId = transfer.DestinationAccountId,
                Amount = transaction.Amount
            };

            var logdes = transaction.Type == TransactionType.SameAccount ? "at this bank" : $"at bank {transaction.DestinationBankId}";
            _logger.LogInformation($"Transfer to {bank} bank from account {_accountId} to {transaction.DestinationAccountId} at {logdes}");

            return Ok(receipt);
        }

        // POST transfers/receive
        [HttpPost("receive")]
        public IActionResult Receive([FromBody]ReceiveTransferViewModel transfer)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var transaction = new Transaction()
            {
                Date = DateTime.Now,
                Description = "Transfer received from another bank",
                AccountId = transfer.DestinationAccountId,
                SourceBankId = transfer.SourceBankId,
                SourceAccountId = transfer.SourceAccountId,
                Amount = transfer.Amount,
                Type = TransactionType.AnotherBankRealTime,
                Status = TransactionStatus.Pending
            };

            _repos.Transactions.Add(transaction);
            _repos.SaveAndApply();

            _command.Execute(transaction.TransactionId);

            var receipt = new ReceiveTransferReceiptViewModel()
            {
                TransactionId = transaction.TransactionId,
                DestinationBankId = transfer.DestinationBankId,
                DestinationAccountId = transfer.DestinationAccountId,
                SourceBankId = transfer.SourceBankId,
                SourceAccountId = transfer.SourceAccountId,
                Amount = transaction.Amount
            };

            _logger.LogInformation($"Transfer from bank {transaction.SourceBankId} from account {_accountId} to {transaction.DestinationAccountId}");

            return Ok(receipt);
        }

        private TransactionType? GetType(string value)
        {
            switch (value)
            {
                case "same":
                    return TransactionType.SameBankRealTime;
                case "another":
                    return TransactionType.AnotherBankRealTime;
                default:
                    return null;
            }
        }
    }
}

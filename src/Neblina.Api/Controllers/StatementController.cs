using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neblina.Api.Core;
using Neblina.Api.Models.StatementViewModels;

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

        // GET: balance
        [HttpGet]
        public IActionResult Get()
        {
            var transactions = _repos.Transactions.GetAll(_accountId);
            var balance = new List<StatementItemViewModel>();

            foreach (var transaction in transactions)
                balance.Add(new StatementItemViewModel()
                {
                    TransactionId = transaction.TransactionId,
                    Amount = transaction.Amount,
                    Type = transaction.Type,
                    Status = transaction.Status
                });

            return Ok(balance);
        }
    }
}

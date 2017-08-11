using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neblina.Binder.Api.Persistence;
using Neblina.Binder.Api.Core.Models;
using Neblina.Binder.Api.Core;
using Neblina.Binder.Api.Core.Services;
using Microsoft.Extensions.Logging;

namespace Neblina.Binder.Api.Controllers
{
    [Route("banks")]
    public class BankController : Controller
    {
        private IUnitOfWork _repos;
        private IBankStatusService _bankStatus;
        private ILogger _logger;

        public BankController(IUnitOfWork repos, IBankStatusService bankStatus, ILogger<BankController> logger)
        {
            _repos = repos;
            _bankStatus = bankStatus;
            _logger = logger;
        }

        // GET banks
        [HttpGet]
        public IActionResult Get()
        {
            var banks = _repos.Banks.GetAll();

            return Ok(banks);
        }

        // GET banks/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var bank = _repos.Banks.Get(id);

            _logger.LogInformation($"Someone asked for bank {id}");

            return Ok(bank);
        }

        // GET banks/retry/5
        [HttpGet("retry/{id}")]
        public IActionResult Retry(int id)
        {
            var bank = _repos.Banks.Get(id);

            if (!_bankStatus.ServerIsRunning(bank.StatusUrl))
            {
                _repos.Banks.Remove(bank);
                _repos.SaveAndApply();
                return NotFound();
            }

            return Ok(bank);
        }

        // POST banks
        [HttpPost]
        public IActionResult Post([FromBody]Bank bank)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!_bankStatus.ServerIsRunning(bank.StatusUrl))
                return BadRequest();

            var saved = _repos.Banks.Get(bank.BankId);

            if (saved == null)
                _repos.Banks.Add(bank);
            else
            {
                saved.Name = bank.Name;
                saved.ReceiveUrl = bank.ReceiveUrl;
                saved.StatusUrl = bank.StatusUrl;
            }

            _repos.SaveAndApply();

            _logger.LogInformation($"Bank {bank.Name} is now registered");

            return Ok(new { bank.BankId });
        }

        // DELETE banks/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var bank = _repos.Banks.Get(id);

            if (bank == null)
                return NotFound();

            _repos.Banks.Remove(bank);
            _repos.SaveAndApply();

            _logger.LogInformation($"Bank {bank.Name} is now not registered");

            return Ok();
        }
    }
}

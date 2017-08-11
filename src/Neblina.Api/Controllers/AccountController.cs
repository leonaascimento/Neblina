using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Neblina.Api.Core;
using Neblina.Api.Core.Models;

namespace Neblina.Api.Controllers
{
    [Route("accounts")]
    public class AccountController : Controller
    {
        private IUnitOfWork _repos;

        private ILogger _logger;

        public AccountController(IUnitOfWork repos, ILogger<AccountController> logger)
        {
            _repos = repos;
            _logger = logger;
        }

        // GET accounts
        [HttpGet]
        public IActionResult Get()
        {
            var accounts = _repos.Accounts.GetAll();
            _logger.LogInformation($"Someone asked for all accounts");

            return Ok(accounts);
        }

        // GET accounts/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var account = _repos.Accounts.Get(id);

            _logger.LogInformation($"Someone asked for account {id}");
            return Ok(account);
        }

        // POST accounts
        [HttpPost]
        public IActionResult Post([FromBody]Account account)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _repos.Accounts.Add(account);
            _repos.SaveAndApply();

            _logger.LogInformation($"Someone added an Account");

            return Ok();
        }

        // DELETE accounts/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var account = _repos.Accounts.Get(id);

            if (account == null)
                return NotFound();

            _repos.Accounts.Remove(account);
            _repos.SaveAndApply();

            _logger.LogInformation($"Someone deleted account {id}");

            return Ok();
        }
    }
}

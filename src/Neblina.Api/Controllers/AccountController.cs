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

        public AccountController(IUnitOfWork repos)
        {
            _repos = repos;
        }

        // GET accounts
        [HttpGet]
        public IActionResult Get()
        {
            var accounts = _repos.Accounts.GetAll();

            return Ok(accounts);
        }

        // GET accounts/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var account = _repos.Accounts.Get(id);

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

            return Ok();
        }
    }
}

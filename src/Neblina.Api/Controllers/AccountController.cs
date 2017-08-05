using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neblina.Api.Persistence;
using Neblina.Api.Models;
using Microsoft.Extensions.Logging;

namespace Neblina.Api.Controllers
{
    [Route("accounts")]
    public class AccountController : Controller
    {
        private BankingContext _context;

        public AccountController(BankingContext context)
        {
            _context = context;
        }

        // GET accounts
        [HttpGet]
        public IActionResult Get()
        {
            var accounts = _context.Accounts.ToList();

            return Ok(accounts);
        }

        // GET accounts/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var account = _context.Accounts.Find(id);

            return Ok(account);
        }

        // POST accounts
        [HttpPost]
        public IActionResult Post([FromBody]Account account)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _context.Accounts.Add(account);
            _context.SaveChanges();

            return Ok();
        }

        // DELETE accounts/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var account = _context.Accounts.Find(id);

            if (account == null)
                return NotFound();

            _context.Accounts.Remove(account);
            _context.SaveChanges();

            return Ok();
        }
    }
}

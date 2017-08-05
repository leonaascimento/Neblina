using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neblina.Binder.Api.Persistence;
using Neblina.Binder.Api.Models;

namespace Neblina.Binder.Api.Controllers
{
    [Route("banks")]
    public class BankController : Controller
    {
        private BinderContext _context;

        public BankController(BinderContext context)
        {
            _context = context;
        }

        // GET banks
        [HttpGet]
        public IActionResult Get()
        {
            var banks = _context.Banks.ToList();

            return Ok(banks);
        }

        // GET banks/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var bank = _context.Banks.Find(id);

            return Ok(bank);
        }

        // POST banks
        [HttpPost]
        public IActionResult Post([FromBody]Bank bank)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _context.Banks.Add(bank);
            _context.SaveChanges();

            return Ok();
        }

        // DELETE banks/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var bank = _context.Banks.Find(id);

            if (bank == null)
                return NotFound();

            _context.Banks.Remove(bank);
            _context.SaveChanges();

            return Ok();
        }
    }
}

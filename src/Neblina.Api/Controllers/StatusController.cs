using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Neblina.Api.Controllers
{
    [Route("status")]
    public class StatusController : Controller
    {
        // GET: status
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("I'm OK!");
        }
    }
}

using Neblina.Binder.Api.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Binder.Api.Services
{
    public class BankStatusService : IBankStatusService
    {
        public bool ServerIsRunning(string statusUrl)
        {
            return true;
        }
    }
}

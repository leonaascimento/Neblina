using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Binder.Api.Core.Services
{
    public interface IBankStatusService
    {
        bool ServerIsRunning(string statusUrl);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Models
{
    public enum TransactionStatus
    {
        Pending,
        Authorized,
        Successful,
        Canceled,
        Aborted,
        Denied,
        Failed
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Communicators
{
    public class BankCacheEntry
    {
        private DateTime _inserted;

        public BankCacheEntry()
        {
            _inserted = DateTime.Now;
        }

        public int BankId { get; set; }
        public string Name { get; set; }
        public string ReceiveUrl { get; set; }
        public string StatusUrl { get; set; }
        public bool Old => (DateTime.Now - _inserted) > TimeSpan.FromMinutes(3);

    }
}

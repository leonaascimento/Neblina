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

        public int bankId { get; set; }
        public string name { get; set; }
        public string receiveUrl { get; set; }
        public string statusUrl { get; set; }
        public bool Old => (DateTime.Now - _inserted) > TimeSpan.FromMinutes(3);

    }
}

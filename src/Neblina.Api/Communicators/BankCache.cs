using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Communicators
{
    public class BankCache
    {
        Dictionary<int, BankCacheEntry> _cache;

        public BankCache()
        {
            _cache = new Dictionary<int, BankCacheEntry>();
        }

        public void Add(BankCacheEntry entry)
        {
            _cache.Add(entry.BankId, entry);
        }

        public bool TryGetValue(int id, out BankCacheEntry value)
        {
            var found = _cache.TryGetValue(id, out value);

            if (value.Old)
            {
                found = false;
                _cache.Remove(id);
            }

            return false;
        }

    }
}

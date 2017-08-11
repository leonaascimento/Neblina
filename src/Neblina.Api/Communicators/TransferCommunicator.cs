using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Net;

namespace Neblina.Api.Communicators
{
    public class TransferCommunicator
    {
        private HttpClient _client;
        private BankCache _cache;

        public TransferCommunicator(HttpClient client, BankCache cache)
        {
            _client = client;
            _cache = cache;
        }

        public async Task<bool> SendToDestination(int bankId)
        {
            BankCacheEntry entry;

            var found = _cache.TryGetValue(bankId, out entry);

            if (!found)
            {
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var res = await _client.GetStringAsync($"http://localhost:52203/banks/{bankId}");

                // TODO: serializar resultado da entry
            }

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _client.PostAsync(entry.ReceiveUrl, new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject("objeto"), Encoding.UTF8, "application/json"));

            return true;
        }
    }
}

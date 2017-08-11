using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Net;
using Neblina.Api.Core.Communicators;
using Neblina.Api.Core.Models;
using Neblina.Api.Core;

namespace Neblina.Api.Communicators
{
    public class TransferCommunicator : ITransferCommunicator
    {
        private readonly IUnitOfWork _repos;
        private HttpClient _client;
        private BankCache _cache;

        public TransferCommunicator(HttpClient client, BankCache cache, IUnitOfWork repos)
        {
            _repos = repos;
            _client = client;
            _cache = cache;
        }

        public bool Execute(int id, int tries = 3, int waitInterval = 100)
        {
            var transaction = _repos.Transactions.Get(id);

            return true;
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

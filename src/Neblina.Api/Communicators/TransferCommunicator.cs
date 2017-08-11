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
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
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

            return SendToDestination(transaction.DestinationBankId, transaction);
        }

        public bool SendToDestination(int bankId, Transaction transaction)
        {
            BankCacheEntry entry;

            var found = _cache.TryGetValue(bankId, out entry);

            if (!found)
            {
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // TODO: serializar resultado da entry

                var res = _client.GetAsync($"http://localhost:52203/banks/{bankId}").Result;
                var stream = res.Content.ReadAsStreamAsync().Result;
                var serializer = new DataContractJsonSerializer(typeof(BankCacheEntry));
                entry = (BankCacheEntry)serializer.ReadObject(stream);

                _cache.Add(entry);
            }

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = _client.PostAsync(entry.ReceiveUrl, new StringContent(JsonConvert.SerializeObject(transaction), Encoding.UTF8, "application/json")).Result;

            return response.IsSuccessStatusCode;
        }
    }
}

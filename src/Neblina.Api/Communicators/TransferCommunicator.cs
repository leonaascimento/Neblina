﻿using System;
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
using Neblina.Api.Core.Commands;

namespace Neblina.Api.Communicators
{
    public class TransferCommunicator : ITransferCommunicator
    {
        private readonly IUnitOfWork _repos;
        private readonly ICreditCommand _creditCommand;
        private readonly RegisterBank _registration;
        private HttpClient _client;
        private BankCache _cache;

        public TransferCommunicator(HttpClient client, BankCache cache, RegisterBank registration, IUnitOfWork repos, ICreditCommand creditCommand)
        {
            _repos = repos;
            _creditCommand = creditCommand;
            _registration = registration;
            _client = client;
            _cache = cache;
        }

        public bool Execute(int id, int tries = 3, int waitInterval = 100)
        {
            var transaction = _repos.Transactions.Get(id);

            var next = false;

            if (transaction.Type == TransactionType.SameBankRealTime)
            {
                var newTran = new Transaction()
                {
                    Date = DateTime.Now,
                    Description = "Transfer received from same bank",
                    AccountId = transaction.DestinationAccountId,
                    SourceBankId = transaction.SourceBankId,
                    SourceAccountId = transaction.SourceAccountId,
                    DestinationBankId = transaction.DestinationBankId,
                    DestinationAccountId = transaction.DestinationAccountId,
                    Amount = transaction.Amount * -1,
                    Type = TransactionType.SameBankRealTime,
                    Status = TransactionStatus.Successful
                };

                _repos.Transactions.Add(newTran);
                _repos.SaveAndApply();

                next = _creditCommand.Execute(newTran.TransactionId);
            }
            else
                next = SendToDestination(transaction.DestinationBankId, transaction);

            return next;
        }

        public bool SendToDestination(int bankId, Transaction transaction)
        {
            BankCacheEntry entry;

            var found = _cache.TryGetValue(bankId, out entry);

            if (!found)
            {
                try
                {
                    _client.DefaultRequestHeaders.Accept.Clear();
                    _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // TODO: serializar resultado da entry

                    var res = _client.GetAsync($"{_registration.BinderAddress}/banks/{bankId}").Result;
                    var stream = res.Content.ReadAsStreamAsync().Result;
                    var serializer = new DataContractJsonSerializer(typeof(BankCacheEntry));
                    entry = (BankCacheEntry)serializer.ReadObject(stream);

                    _cache.Add(entry);
                }
                catch
                {
                    return false;
                }
            }

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var message = new
            {
                sourceBankId = transaction.SourceBankId,
                sourceAccountId = transaction.SourceAccountId,
                destinationBankId = transaction.DestinationBankId,
                destinationAccountId = transaction.DestinationAccountId,
                amount = transaction.Amount * -1
            };

            var serial = JsonConvert.SerializeObject(message);
            var response = _client.PostAsync(entry.receiveUrl, new StringContent(serial, Encoding.UTF8, "application/json")).Result;

            return response.IsSuccessStatusCode;
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Neblina.Api.Communicators
{
    public class RegisterBank
    {
        private readonly int _bankId;
        private readonly string _bankName;
        private readonly string _bankReceiveUrl;
        private readonly string _bankStatusUrl;
        private readonly string _binderAddress;

        public RegisterBank(string bankId, string bankName, string bankReceiveUrl, string bankStatusUrl, string binderAddress)
        {
            _bankId = int.Parse(bankId);
            _bankName = bankName;
            _bankReceiveUrl = bankReceiveUrl;
            _bankStatusUrl = bankStatusUrl;
            _binderAddress = binderAddress;
        }

        public int BankId => _bankId;
        public string BankName => _bankName;
        public string BankReceiveUrl => _bankReceiveUrl;
        public string BankStatusUrl => _bankStatusUrl;
        public string BinderAddress => _binderAddress;

        public void Register()
        {
            var message = new
            {
                bankId = BankId,
                name = BankName,
                receiveUrl = BankReceiveUrl,
                statusUrl = BankStatusUrl,
            };

            using (var client = new HttpClient())
            {
                var serial = JsonConvert.SerializeObject(message);
                var response = client.PostAsync($"{BinderAddress}/banks", new StringContent(serial, Encoding.UTF8, "application/json")).Result;
            }
        }

        public void Deregister()
        {
            using (var client = new HttpClient())
            {
                var result = client.DeleteAsync($"{BinderAddress}/banks/{BankId}").Result;
            }
        }
    }
}

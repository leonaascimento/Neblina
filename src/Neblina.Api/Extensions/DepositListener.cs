using Neblina.Api.Persistence;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Neblina.Api.Core.Models;

namespace Neblina.Api.Extensions
{
    public class DepositListener
    {
        private readonly ConnectionFactory _factory;
        private readonly BankingContext _context;

        public DepositListener(ConnectionFactory factory, BankingContext context)
        {
            _context = context;
            _factory = factory;
        }

        public void Run()
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "deposits", durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.Received += async (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);

                        await Task.Run(() =>
                        {
                            using (var t = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                            {
                                try
                                {
                                    var transaction = _context.Transactions.Find(int.Parse(message));
                                    var account = _context.Accounts.Find(transaction.AccountId);

                                    account.Balance += transaction.Amount;
                                    transaction.Status = TransactionStatus.Successful;

                                    _context.SaveChanges();

                                    t.Commit();
                                }
                                catch
                                {
                                    // TODO: 
                                }
                            }
                        });
                    };
                    channel.BasicConsume(queue: "deposits", autoAck: true, consumer: consumer);

                    Console.ReadLine();
                }
            }
        }
    }
}

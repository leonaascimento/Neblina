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
using Neblina.Api.Core.Commands;

namespace Neblina.Api.Listeners
{
    public class TransferListener
    {
        private readonly ConnectionFactory _factory;
        private readonly ITransferCommand _transferCommand;

        public TransferListener(ConnectionFactory factory, ITransferCommand transferCommand)
        {
            _transferCommand = transferCommand;
            _factory = factory;
        }

        public void Run()
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "transfers", durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.Received += async (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);

                        await Task.Run(() =>
                        {
                            _transferCommand.Execute(int.Parse(message));
                        });
                    };
                    channel.BasicConsume(queue: "transfers", autoAck: true, consumer: consumer);

                    Console.ReadLine();
                }
            }
        }
    }
}

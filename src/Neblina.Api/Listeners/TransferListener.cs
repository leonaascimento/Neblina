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
        private IConnection _connection;
        private IModel _channel;

        public TransferListener(ConnectionFactory factory, ITransferCommand transferCommand)
        {
            _transferCommand = transferCommand;
            _factory = factory;
        }

        public void Register()
        {
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "transfers", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);

                await Task.Run(() =>
                {
                    _transferCommand.Execute(int.Parse(message));
                });
            };
            _channel.BasicConsume(queue: "transfers", autoAck: true, consumer: consumer);
        }

        public void Deregister()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}

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
using Neblina.Api.Core.Communicators;

namespace Neblina.Api.Listeners
{
    public class TransferListener
    {
        private readonly ConnectionFactory _factory;
        private readonly ITransferCommand _transferCommand;
        private readonly ITransferCommunicator _transferCommunicator;
        private IConnection _connection;
        private IModel _channel;

        public TransferListener(ConnectionFactory factory, ITransferCommand transferCommand, ITransferCommunicator transferCommunicator)
        {
            _transferCommand = transferCommand;
            _transferCommunicator = transferCommunicator;
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
                    var id = int.Parse(message);

                    _transferCommand.Execute(id);
                    var result = _transferCommunicator.Execute(id);

                    // TODO escrever apply ou rollback
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

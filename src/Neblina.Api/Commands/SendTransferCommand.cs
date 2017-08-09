using Neblina.Api.Core.Commands;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neblina.Api.Commands
{
    public class SendTransferCommand : ISendTransferCommand
    {
        private readonly ConnectionFactory _factory;

        public SendTransferCommand(ConnectionFactory factory)
        {
            _factory = factory;
        }

        public void Enqueue(int id)
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "transfers", durable: false, exclusive: false, autoDelete: false, arguments: null);

                    string message = id.ToString();
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "", routingKey: "transfers", basicProperties: null, body: body);
                }
            }
        }
    }
}

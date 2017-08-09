using Neblina.Api.Core.Commands;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neblina.Api.Commands
{
    public class DepositCommand : IDepositCommand
    {
        private readonly ConnectionFactory _factory;

        public DepositCommand(ConnectionFactory factory)
        {
            _factory = factory;
        }

        public void Enqueue(int id)
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "deposits", durable: false, exclusive: false, autoDelete: false, arguments: null);

                    string message = id.ToString();
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "", routingKey: "deposits", basicProperties: null, body: body);
                }
            }
        }
    }
}

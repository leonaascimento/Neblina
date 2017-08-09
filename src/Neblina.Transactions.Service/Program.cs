using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace Neblina.Transactions.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<ConnectionFactory, ConnectionFactory>(factory =>
                {
                    return new ConnectionFactory() { HostName = "localhost" };
                })
                .AddSingleton<Startup, Startup>()
                .BuildServiceProvider();

            //configure console logging
            serviceProvider
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            logger.LogDebug("Starting application");

            //do the actual work here
            var bar = serviceProvider.GetService<Startup>();
            bar.Run();

            logger.LogDebug("All done!");
        }
    }
}
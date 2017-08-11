using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Neblina.Api.Persistence;
using Neblina.Api.Core;
using Neblina.Api.Core.Commands;
using Neblina.Api.Persistence.Commands;
using RabbitMQ.Client;
using Neblina.Api.Extensions;
using Neblina.Api.Core.Dispatchers;
using Neblina.Api.Dispatchers;
using Neblina.Api.Listeners;
using System.Net.Http;
using Neblina.Api.Core.Communicators;
using Neblina.Api.Communicators;

namespace Neblina.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<BankingContext>(
                options => options.UseSqlite("Data Source=banking.db"));

            services.AddCors();
            services.AddMvc();

            // Add application services.
            services.AddSingleton<ConnectionFactory, ConnectionFactory>(factory =>
            {
                return new ConnectionFactory() { HostName = "localhost", DispatchConsumersAsync = true };
            });
            services.AddSingleton<DepositListener, DepositListener>();
            services.AddSingleton<TransferListener, TransferListener>();
            services.AddSingleton<BankCache, BankCache>();
            services.AddSingleton<HttpClient, HttpClient>();
            services.AddSingleton<ITransferCommunicator, TransferCommunicator>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ICreditCommand, CreditCommand>();
            services.AddTransient<IDebitCommand, DebitCommand>();
            services.AddTransient<ITransferCommand, TransferCommand>();
            services.AddTransient<ITransferDispatcher, TransferDispatcher>();
            services.AddTransient<IDepositDispatcher, DepositDispatcher>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod());

            app.UseRabbitListener();

            app.UseMvc();
        }
    }
}

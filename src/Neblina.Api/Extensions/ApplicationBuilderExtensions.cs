using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Neblina.Api.Listeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static DepositListener DepositListener { get; set; }
        public static TransferListener TransferListener { get; set; }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            DepositListener = app.ApplicationServices.GetService<DepositListener>();
            TransferListener = app.ApplicationServices.GetService<TransferListener>();

            var life = app.ApplicationServices.GetService<IApplicationLifetime>();

            life.ApplicationStarted.Register(OnStarted);

            //press Ctrl+C to reproduce if your app runs in Kestrel as a console app
            life.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            DepositListener.Register();
            TransferListener.Register();
        }

        private static void OnStopping()
        {
            DepositListener.Deregister();
            TransferListener.Deregister();
        }
    }
}

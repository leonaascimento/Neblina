using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Neblina.Api.Communicators;
using Neblina.Api.Listeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neblina.Api.Extensions
{
    public static class AutoconfigExtensions
    {
        public static RegisterBank RegisterBank { get; set; }

        public static IApplicationBuilder UseAutoconfig(this IApplicationBuilder app)
        {
            RegisterBank = app.ApplicationServices.GetService<RegisterBank>();

            var life = app.ApplicationServices.GetService<IApplicationLifetime>();

            life.ApplicationStarted.Register(OnStarted);

            //press Ctrl+C to reproduce if your app runs in Kestrel as a console app
            life.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            RegisterBank.Register();
        }

        private static void OnStopping()
        {
            RegisterBank.Deregister();
        }
    }
}

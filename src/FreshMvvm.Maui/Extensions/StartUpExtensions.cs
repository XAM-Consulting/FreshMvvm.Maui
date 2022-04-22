using System;
using FreshMvvm.Maui.IOC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;

namespace FreshMvvm.Maui.Extensions
{
    public static class StartUpExtensions
    {
        /// <summary>
        /// Registers the services in the service collection with the page resolver
        /// </summary>
        /// <param name="sc"></param>
        public static void UseFreshMvvm(this MauiApp app)
        {
            DependancyService.RegisterServiceProvider(app.Services.GetService<IServiceProvider>());
        }
    }
}


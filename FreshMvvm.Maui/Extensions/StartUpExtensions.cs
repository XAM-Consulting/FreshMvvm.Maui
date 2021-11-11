using System;
using FreshMvvm.Maui.IOC;
using Microsoft.Extensions.DependencyInjection;

namespace FreshMvvm.Maui.Extensions
{
    public static class StartUpExtensions
    {
        /// <summary>
        /// Registers the services in the service collection with the page resolver
        /// </summary>
        /// <param name="sc"></param>
        public static void UseFreshMvvm(this IServiceCollection sc)
        {
            var sp = sc.BuildServiceProvider();
            DependancyService.RegisterServiceProvider(sp);
        }
    }
}


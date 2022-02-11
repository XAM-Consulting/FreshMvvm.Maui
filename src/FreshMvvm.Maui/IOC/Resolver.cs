using System;
using Microsoft.Extensions.DependencyInjection;

namespace FreshMvvm.Maui.IOC
{
    public static class DependancyService
    {
        private static IServiceScope scope;

        /// <summary>
        /// REgisters the service provider and creates a dependency scope
        /// </summary>
        /// <param name="sp"></param>
        public static void RegisterServiceProvider(IServiceProvider sp)
        {
            scope = sp.CreateScope();
        }

        /// <summary>
        /// Returns a resolved instance of the requested type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>() where T : class
        {
            var result = scope.ServiceProvider.GetRequiredService<T>();

            return result;
        }

        public static object Resolve(Type serviceType)
        {
            var result = scope.ServiceProvider.GetRequiredService(serviceType);

            return result;
        }
    }
}


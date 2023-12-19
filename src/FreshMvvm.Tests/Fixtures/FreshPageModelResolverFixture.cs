using System;
using FluentAssertions.Common;
using FreshMvvm.Maui;
using FreshMvvm.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace FreshMvvm.Tests.Fixtures
{
	[TestFixture]
	public class FreshPageModelResolverFixture
    {
        [TestCase]
		public void Test_ResolvePageModel_Not_Found()
        {
            RegisterServices((services) => { /* Register nothing */ });

            Assert.Throws<InvalidOperationException>(() =>
			{
				FreshPageModelResolver.ResolvePageModel<MockFreshBasePageModel>();
			});
		}

		[TestCase]
		public void Test_ResolvePageModel()
        {
            RegisterServices((services) =>
            {
                services.AddTransient<MockContentPageModel>();
                services.AddTransient<MockContentPage>();
            });

            var page = FreshPageModelResolver.ResolvePageModel<MockContentPageModel>();
			var context = page.BindingContext as MockContentPageModel;

			Assert.IsNotNull(context);
			Assert.IsNotNull(context.CurrentPage);
			Assert.IsNotNull(context.CoreMethods);
		}

		[TestCase("test data")]
		public void Test_ResolvePageModel_With_Init(object data)
        {
            RegisterServices((services) =>
            {
                services.AddTransient<MockContentPageModel>();
                services.AddTransient<MockContentPage>();
            });

            var page = FreshPageModelResolver.ResolvePageModel<MockContentPageModel>(data);
			var context = page.BindingContext as MockContentPageModel;

			Assert.IsNotNull(context);
			Assert.IsNotNull(context.CurrentPage);
			Assert.IsNotNull(context.CoreMethods);
			Assert.AreSame(data, context.Data);
		}

		void RegisterServices(Action<IServiceCollection> registerServices)
		{
            var services = new ServiceCollection();
            registerServices(services);
            var serivceProvider = services.BuildServiceProvider();
            Maui.IOC.DependancyService.RegisterServiceProvider(serivceProvider);
        }
	}
}


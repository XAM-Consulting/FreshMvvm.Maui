﻿using System.Threading.Tasks;
using FreshMvvm.Tests.Mocks;
using NSubstitute;
using NUnit.Framework;
using Microsoft.Maui.Controls;
using FreshMvvm.Maui;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FreshMvvm.Tests.Fixtures
{
	[TestFixture]
	class PageModelCoreMethodsFixture
	{
	    PageModelCoreMethods _coreMethods;
	    IFreshNavigationService _navigationMock;
	    Page _page;
	    FreshBasePageModel _pageModel;

        void SetupFirstNavigationAndPage()
        {
            _navigationMock = Substitute.For<IFreshNavigationService>();
	        
	        _page = FreshPageModelResolver.ResolvePageModel<MockContentPageModel>();
	        _pageModel = _page.BindingContext as MockContentPageModel;
			_pageModel.SetCurrentNavigationService(_navigationMock);

			_coreMethods = new PageModelCoreMethods(_page, _pageModel);
        }

        [Test]
	    public async Task model_property_populated_by_action()
	    {
            RegisterServices((services) =>
            {
                services.AddTransient<MockContentPageModel>();
                services.AddTransient<MockContentPage>();
                services.AddTransient<MockItemPageModel>();
                services.AddTransient<MockItemPage>();
            });

            SetupFirstNavigationAndPage();

            const string item = "asj";
	        await _coreMethods.PushPageModel<MockItemPageModel>(pm => pm.Item = item);

	        _navigationMock.Received().PushPage(Arg.Any<Page>(), Arg.Is<MockItemPageModel>(o => o.Item == item), Arg.Any<bool>(), Arg.Any<bool>());
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

﻿using System;
using FreshMvvm.Maui;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreshMvvmApp
{
	/// <summary>
	/// This is a sample custom implemented Navigation. It combines a MasterDetail and a TabbedPage.
	/// </summary>
    public class CustomImplementedNav : FlyoutPage, IFreshNavigationService
	{
		FreshTabbedNavigationContainer _tabbedNavigationPage;
		Page _contactsPage, _quotesPage;

		public CustomImplementedNav ()
		{	
			SetupTabbedPage ();
			CreateMenuPage ("Menu");
		}

		void SetupTabbedPage()
		{
			_tabbedNavigationPage = new FreshTabbedNavigationContainer ();
			_contactsPage = _tabbedNavigationPage.AddTab<ContactListPageModel> ("Contacts", "contacts.png");
			_quotesPage = _tabbedNavigationPage.AddTab<QuoteListPageModel> ("Quotes", "document.png");
			this.Detail = _tabbedNavigationPage;
		}

		protected void CreateMenuPage(string menuPageTitle)
		{
			var _menuPage = new ContentPage ();
			_menuPage.Title = menuPageTitle;
			var listView = new ListView();

			listView.ItemsSource = new string[] { "Contacts", "Quotes", "Modal Demo" };

			listView.ItemSelected += async (sender, args) =>
			{

				switch ((string)args.SelectedItem) {
				case "Contacts":
					_tabbedNavigationPage.CurrentPage = _contactsPage;
					break;
				case "Quotes":
					_tabbedNavigationPage.CurrentPage = _quotesPage;
					break;
				case "Modal Demo":
                    var modalPage = FreshPageModelResolver.ResolvePageModel<ModalPageModel>();
					await PushPage(modalPage, null, true);
					break;
				default:
				break;
				}

				IsPresented = false;
			};

			_menuPage.Content = listView;

			Flyout = new NavigationPage(_menuPage) { Title = "Menu" };
		}

        public virtual async Task PushPage (Page page, FreshBasePageModel model, bool modal = false, bool animated = true)
		{
			if (modal)
                await Navigation.PushModalAsync (new NavigationPage(page), animated);
			else
                await ((NavigationPage)_tabbedNavigationPage.CurrentPage).PushAsync (page, animated); 
		}

        public virtual async Task PopPage (bool modal = false, bool animate = true)
		{
			if (modal)
				await Navigation.PopModalAsync ();
			else
				await ((NavigationPage)_tabbedNavigationPage.CurrentPage).PopAsync (); 
		}

        public virtual async Task PopToRoot (bool animate = true)
        {
            await ((NavigationPage)_tabbedNavigationPage.CurrentPage).PopToRootAsync (animate);
        }

        public void NotifyChildrenPageWasPopped()
        {
            if (Flyout is NavigationPage)
                ((NavigationPage)Flyout).NotifyAllChildrenPopped();
            foreach (var page in _tabbedNavigationPage.Children)
            {
                if (page is NavigationPage)
                    ((NavigationPage)page).NotifyAllChildrenPopped();
            }
        }

        public Task<FreshBasePageModel> SwitchSelectedRootPageModel<T> () where T : FreshBasePageModel
        {
            if (_contactsPage.GetModel ().GetType ().FullName == typeof (T).FullName) {
                _tabbedNavigationPage.CurrentPage = _contactsPage;
                return Task.FromResult(_contactsPage.GetModel ());
            }

            if (_quotesPage.GetModel ().GetType ().FullName == typeof (T).FullName) {
                _tabbedNavigationPage.CurrentPage = _quotesPage;
                return Task.FromResult(_quotesPage.GetModel ());
            }

            throw new Exception ("Cannot do this");
        }
    }
}


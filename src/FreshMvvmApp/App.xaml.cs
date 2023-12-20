using FreshMvvm.Maui;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Application = Microsoft.Maui.Controls.Application;

namespace FreshMvvmApp
{
    public partial class App : Application
    {
        public App()
        { 
            MainPage = new NavigationPage(new LaunchPage(this));
        }

        public void LoadBasicNav()
        {
            var page = FreshPageModelResolver.ResolvePageModel<MainMenuPageModel>();
            var basicNavContainer = new FreshNavigationContainer(page);
            MainPage = basicNavContainer;
        }

        public void LoadMasterDetail()
        {
            var masterDetailNav = new FreshMasterDetailNavigationContainer();
            masterDetailNav.Init("Menu", "menu");
            masterDetailNav.AddPage<ContactListPageModel>("Contacts", null);
            masterDetailNav.AddPage<QuoteListPageModel>("Quotes", null);
            MainPage = masterDetailNav;
        }

        public void LoadTabbedNav()
        {
            var tabbedNavigation = new FreshTabbedNavigationContainer();
            tabbedNavigation.AddTab<ContactListPageModel>("Contacts", "contacts", null);
            tabbedNavigation.AddTab<QuoteListPageModel>("Quotes", "document", null);
            MainPage = tabbedNavigation;
        }

        public void LoadFOTabbedNav()
        {
            var tabbedNavigation = new FreshTabbedFONavigationContainer("CRM");
            tabbedNavigation.AddTab<ContactListPageModel>("Contacts", "contacts", null);
            tabbedNavigation.AddTab<QuoteListPageModel>("Quotes", "document", null);
            MainPage = tabbedNavigation;
        }

        public void LoadCustomNav()
        {
            MainPage = new CustomImplementedNav();
        }

        public void LoadMultipleNavigation()
        {
            var masterDetailsMultiple = new Microsoft.Maui.Controls.FlyoutPage(); //generic master detail page

            //we setup the first navigation container with ContactList
            var contactListPage = FreshPageModelResolver.ResolvePageModel<ContactListPageModel>();
            contactListPage.Title = "Contact List";
            //we setup the first navigation container with name MasterPageArea
            var masterPageArea = new FreshNavigationContainer(contactListPage);
            masterPageArea.Title = "Menu";

            masterDetailsMultiple.Flyout = masterPageArea; //set the first navigation container to the Master

            //we setup the second navigation container with the QuoteList 
            var quoteListPage = FreshPageModelResolver.ResolvePageModel<QuoteListPageModel>();
            quoteListPage.Title = "Quote List";
            //we setup the second navigation container with name DetailPageArea
            var detailPageArea = new FreshNavigationContainer(quoteListPage);

            masterDetailsMultiple.Detail = detailPageArea; //set the second navigation container to the Detail

            MainPage = masterDetailsMultiple;
        }
    }
}

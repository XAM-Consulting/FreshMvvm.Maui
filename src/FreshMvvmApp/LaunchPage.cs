using FreshMvvm.Maui;

namespace FreshMvvmApp
{
    public class LaunchPage : ContentPage
    {
        public LaunchPage ()
        {
            Title = "Select Sample";
            var list = new ListView ();
            list.ItemsSource = new List<string> {
                "Basic Navigation",
                "Master Detail",
                "Tabbed Navigation",
                "Custom Navigation",
                "Tabbed (FO) Navigation",
                "Multiple Navigation"
            };
            list.ItemTapped += (object sender, ItemTappedEventArgs e) => {
                if ((string)e.Item == "Basic Navigation")
                    LoadBasicNav ();
                else if ((string)e.Item == "Master Detail")
                    LoadMasterDetail ();
                else if ((string)e.Item == "Tabbed Navigation")
                    LoadTabbedNav ();
                else if ((string)e.Item == "Tabbed (FO) Navigation")
                    LoadFOTabbedNav ();
                else if ((string)e.Item == "Custom Navigation")
                    LoadCustomNav ();
                else if ((string)e.Item == "Multiple Navigation")
                    LoadMultipleNavigation ();
            };
            Content = list;
        }

        public void LoadBasicNav()
        {
            var page = FreshPageModelResolver.ResolvePageModel<MainMenuPageModel>();
            var basicNavContainer = new FreshNavigationContainer(page);
            Application.Current.MainPage = basicNavContainer;
        }

        public void LoadMasterDetail()
        {
            var masterDetailNav = new FreshMasterDetailNavigationContainer();
            masterDetailNav.Init("Menu", "Menu.png");
            masterDetailNav.AddPage<ContactListPageModel>("Contacts", null);
            masterDetailNav.AddPage<QuoteListPageModel>("Quotes", null);
            Application.Current.MainPage = masterDetailNav;
        }

        public void LoadTabbedNav()
        {
            var tabbedNavigation = new FreshTabbedNavigationContainer();
            tabbedNavigation.AddTab<ContactListPageModel>("Contacts", "contacts.png", null);
            tabbedNavigation.AddTab<QuoteListPageModel>("Quotes", "document.png", null);
            Application.Current.MainPage = tabbedNavigation;
        }

        public void LoadFOTabbedNav()
        {
            var tabbedNavigation = new FreshTabbedFONavigationContainer("CRM");
            tabbedNavigation.AddTab<ContactListPageModel>("Contacts", "contacts.png", null);
            tabbedNavigation.AddTab<QuoteListPageModel>("Quotes", "document.png", null);
            Application.Current.MainPage = tabbedNavigation;
        }

        public void LoadCustomNav()
        {
            Application.Current.MainPage = new CustomImplementedNav();
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

            Application.Current.MainPage = masterDetailsMultiple;
        }

    }
}


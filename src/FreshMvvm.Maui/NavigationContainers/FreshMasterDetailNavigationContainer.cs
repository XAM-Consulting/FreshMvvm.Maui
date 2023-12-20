using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FreshMvvm.Maui
{
    public class FreshMasterDetailNavigationContainer : FlyoutPage, IFreshNavigationService
    {
        List<Page> _pagesInner = new List<Page> ();
        Dictionary<string, Page> _pages = new Dictionary<string, Page> ();
        ContentPage _menuPage;
        ObservableCollection<string> _pageNames = new ObservableCollection<string> ();
	    ListView _listView = new ListView ();

        public Dictionary<string, Page> Pages { get { return _pages; } }
        protected ObservableCollection<string> PageNames { get { return _pageNames; } }

        public void Init (string menuTitle, string menuIcon = null)
        {
            CreateMenuPage (menuTitle, menuIcon);
        }

        public virtual void AddPage<T> (string title, object data = null) where T : FreshBasePageModel
        {
            var page = FreshPageModelResolver.ResolvePageModel<T> (data);
            page.GetModel ().CurrentNavigationService = this;
            _pagesInner.Add(page);
            var navigationContainer = CreateContainerPage (page);
            _pages.Add (title, navigationContainer);
            _pageNames.Add (title);
            if (_pages.Count == 1)
                Detail = navigationContainer;
        }
        public virtual void AddPage(string modelName, string title, object data = null)
        {
            var pageModelType = Type.GetType(modelName);
            var page = FreshPageModelResolver.ResolvePageModel(pageModelType, null);
            page.GetModel().CurrentNavigationService = this;
            _pagesInner.Add(page);
            var navigationContainer = CreateContainerPage(page);
            _pages.Add(title, navigationContainer);
            _pageNames.Add(title);
            if (_pages.Count == 1)
                Detail = navigationContainer;
        }

        internal Page CreateContainerPageSafe (Page page)
        {
            if (page is NavigationPage || page is FlyoutPage || page is TabbedPage)
                return page;

            return CreateContainerPage(page);
        }

        protected virtual Page CreateContainerPage (Page page)
        {
            return new NavigationPage (page);
        }

        protected virtual void CreateMenuPage (string menuPageTitle, string menuIcon = null)
        {
            _menuPage = new ContentPage ();
            _menuPage.Title = menuPageTitle; 
	    
            _listView.ItemsSource = _pageNames;

            _listView.ItemSelected += (sender, args) => {
                if (_pages.ContainsKey ((string)args.SelectedItem)) {
                    Detail = _pages [(string)args.SelectedItem];
                }

                if (!((IFlyoutPageController)this).ShouldShowSplitMode)
                    IsPresented = false;
            };

            _menuPage.Content = _listView;

            var navPage = new NavigationPage (_menuPage) { Title = "Menu" };

            if (!string.IsNullOrEmpty (menuIcon))
            {
                navPage.IconImageSource = menuIcon;                
            }                
            
            Flyout = navPage;
        }

        public Task PushPage (Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
        {
            if (modal)
                return Navigation.PushModalAsync (CreateContainerPageSafe(page));
            return (Detail as NavigationPage).PushAsync (page, animate); //TODO: make this better
		}

		public Task PopPage (bool modal = false, bool animate = true)
		{
            if (modal)
                return Navigation.PopModalAsync (animate);
            return (Detail as NavigationPage).PopAsync (animate); //TODO: make this better            
		}

        public Task PopToRoot (bool animate = true)
        {
            return (Detail as NavigationPage).PopToRootAsync (animate);
        }

        public void NotifyChildrenPageWasPopped()
        {
            if (Flyout is NavigationPage)
                ((NavigationPage)Flyout).NotifyAllChildrenPopped();
            if (Flyout is IFreshNavigationService)
                ((IFreshNavigationService)Flyout).NotifyChildrenPageWasPopped();
            
            foreach (var page in this.Pages.Values)
            {
                if (page is NavigationPage)
                    ((NavigationPage)page).NotifyAllChildrenPopped();
                if (page is IFreshNavigationService)
                    ((IFreshNavigationService)page).NotifyChildrenPageWasPopped();
            }
            if (this.Pages != null && !this.Pages.ContainsValue(Detail) && Detail is NavigationPage)
                ((NavigationPage)Detail).NotifyAllChildrenPopped();
            if (this.Pages != null && !this.Pages.ContainsValue(Detail) && Detail is IFreshNavigationService)
                ((IFreshNavigationService)Detail).NotifyChildrenPageWasPopped();
        }

        public Task<FreshBasePageModel> SwitchSelectedRootPageModel<T>() where T : FreshBasePageModel
        {
            var tabIndex = _pagesInner.FindIndex(o => o.GetModel().GetType().FullName == typeof(T).FullName);

            _listView.SelectedItem = _pageNames[tabIndex];

            return Task.FromResult((Detail as NavigationPage).CurrentPage.GetModel());
        }
    }
}


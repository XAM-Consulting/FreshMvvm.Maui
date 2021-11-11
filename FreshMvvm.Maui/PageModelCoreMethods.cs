using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.Linq;
using FreshMvvm.Maui.IOC;

namespace FreshMvvm.Maui
{
    public class PageModelCoreMethods : IPageModelCoreMethods
    {
        Page _currentPage;
        FreshBasePageModel _currentPageModel;

		public PageModelCoreMethods (Page currentPage, FreshBasePageModel pageModel)
        {
            _currentPage = currentPage;
			_currentPageModel = pageModel;
        }

        public async Task DisplayAlert (string title, string message, string cancel)
        {
            if (_currentPage != null)
                await _currentPage.DisplayAlert (title, message, cancel);
        }

        public async Task<string> DisplayActionSheet (string title, string cancel, string destruction, params string[] buttons)
        {
            if (_currentPage != null)
                return await _currentPage.DisplayActionSheet (title, cancel, destruction, buttons);
            return null;
        }

        public async Task<bool> DisplayAlert (string title, string message, string accept, string cancel)
        {
            if (_currentPage != null)
                return await _currentPage.DisplayAlert (title, message, accept, cancel);	
            return false;
        }

        public async Task PushPageModel<T>(Action<T> setPageModel, bool modal = false, bool animate = true) where T : FreshBasePageModel
        {
            T pageModel = DependancyService.Resolve<T>();

            setPageModel?.Invoke(pageModel);

            await PushPageModel(pageModel, null, modal, animate);
        }

        public async Task PushPageModel<T>(object data, bool modal = false, bool animate = true) where T : FreshBasePageModel
        {
            T pageModel = DependancyService.Resolve<T>();

            await PushPageModel(pageModel, data, modal, animate);
        }

        public async Task PushPageModel<T, TPage> (object data, bool modal = false, bool animate = true) where T : FreshBasePageModel where TPage : Page
        {
            T pageModel = DependancyService.Resolve<T> ();
			TPage page = DependancyService.Resolve<TPage>();
			FreshPageModelResolver.BindingPageModel(data, page, pageModel);
            await PushPageModelWithPage(page, pageModel, data, modal, animate);
        }

        public Task PushPageModel(Type pageModelType, bool animate = true)
        {
            return PushPageModel(pageModelType, null, animate);
        }

        public Task PushPageModel(Type pageModelType, object data, bool modal = false, bool animate = true)
        {
            var pageModel = DependancyService.Resolve(pageModelType) as FreshBasePageModel;

            return PushPageModel(pageModel, data, modal, animate);
        }

        async Task PushPageModel(FreshBasePageModel pageModel, object data, bool modal = false, bool animate = true)
        {
            var page = FreshPageModelResolver.ResolvePageModel(data, pageModel);
            await PushPageModelWithPage(page, pageModel, data, modal, animate);
        }

        async Task PushPageModelWithPage(Page page, FreshBasePageModel pageModel, object data, bool modal = false, bool animate = true)
        {
            pageModel.PreviousPageModel = _currentPageModel; //This is the previous page model because it's push to a new one, and this is current
            pageModel.CurrentNavigationService = _currentPageModel.CurrentNavigationService;

            if (pageModel.PreviousNavigationService != null)
                pageModel.PreviousNavigationService = _currentPageModel.PreviousNavigationService;

            if (page is FreshMasterDetailNavigationContainer) 
            {
                await this.PushNewNavigationServiceModal ((FreshMasterDetailNavigationContainer)page, pageModel, animate);
            } 
            else if (page is FreshTabbedNavigationContainer) 
            {
                await this.PushNewNavigationServiceModal ((FreshTabbedNavigationContainer)page, pageModel, animate);
            } 
            else 
            {
                IFreshNavigationService rootNavigation = _currentPageModel.CurrentNavigationService;

                await rootNavigation.PushPage (page, pageModel, modal, animate);
            }
        }

        public async Task PopPageModel (bool modal = false, bool animate = true)
        {
            if (_currentPageModel.IsModalFirstChild)
            {
                await PopModalNavigationService(animate);
            }
            else
            {
                if (modal)
                    this._currentPageModel.RaisePageWasPopped();

                IFreshNavigationService rootNavigation = _currentPageModel.CurrentNavigationService;
                await rootNavigation.PopPage (modal, animate);                
            }
        }

        public async Task PopToRoot(bool animate)
        {
            IFreshNavigationService rootNavigation = _currentPageModel.CurrentNavigationService;
            await rootNavigation.PopToRoot (animate);
        }

        public async Task PopPageModel (object data, bool modal = false, bool animate = true)
        {
            if (_currentPageModel != null && _currentPageModel.PreviousPageModel != null && data != null) {
                _currentPageModel.PreviousPageModel.ReverseInit (data);
            }
            await PopPageModel (modal, animate);
        }

        public Task PushPageModel<T> (bool animate = true) where T : FreshBasePageModel
        {
            return PushPageModel<T> (null, false, animate);
        }

        public Task PushPageModel<T, TPage> (bool animate = true) where T : FreshBasePageModel where TPage : Page
        {
            return PushPageModel<T, TPage> (null, animate);
        }

        public Task PushNewNavigationServiceModal (FreshTabbedNavigationContainer tabbedNavigationContainer, FreshBasePageModel basePageModel = null, bool animate = true)
        {
            var models = tabbedNavigationContainer.TabbedPages.Select (o => o.GetModel ()).ToList();
            if (basePageModel != null)
                models.Add (basePageModel);
            return PushNewNavigationServiceModal (tabbedNavigationContainer, models.ToArray (), animate);
        }

        public Task PushNewNavigationServiceModal (FreshMasterDetailNavigationContainer masterDetailContainer, FreshBasePageModel basePageModel = null, bool animate = true)
        {
            var models = masterDetailContainer.Pages.Select (o => 
                {
                    if (o.Value is NavigationPage)
                        return ((NavigationPage)o.Value).CurrentPage.GetModel ();
                    else
                        return o.Value.GetModel();
                }).ToList();

            if (basePageModel != null)
                models.Add (basePageModel);
            
            return PushNewNavigationServiceModal (masterDetailContainer, models.ToArray(), animate);
        }

        public Task PushNewNavigationServiceModal (IFreshNavigationService newNavigationService, FreshBasePageModel basePageModels, bool animate = true)
        {
            return PushNewNavigationServiceModal (newNavigationService, new FreshBasePageModel[] { basePageModels }, animate);
        }

        public async Task PushNewNavigationServiceModal (IFreshNavigationService newNavigationService, FreshBasePageModel[] basePageModels, bool animate = true)
        {
            var navPage = newNavigationService as Page;
            if (navPage == null)
                throw new Exception ("Navigation service is not Page");

            foreach (var pageModel in basePageModels) {
                pageModel.CurrentNavigationService = newNavigationService;
                pageModel.PreviousNavigationService = _currentPageModel.CurrentNavigationService;
                pageModel.IsModalFirstChild = true;
            }

            IFreshNavigationService rootNavigation = _currentPageModel.CurrentNavigationService;
            await rootNavigation.PushPage (navPage, null, true, animate);
        }

        public void SwitchOutRootNavigation (string navigationServiceName)
        {
            //TODO: switch out root

            //IFreshNavigationService rootNavigation = FreshIOC.Container.Resolve<IFreshNavigationService> (navigationServiceName);

            //if (!(rootNavigation is Page))
            //    throw new Exception("Navigation service is not a page");
            
            
            //Application.Current.MainPage = rootNavigation as Page;
        }

        public async Task PopModalNavigationService(bool animate = true)
        {
            var currentNavigationService = _currentPageModel.CurrentNavigationService;
            currentNavigationService.NotifyChildrenPageWasPopped();

            IFreshNavigationService rootNavigation = _currentPageModel.PreviousNavigationService;
            await rootNavigation.PopPage (true, animate);
        }

        /// <summary>
        /// This method switches the selected main page, TabbedPage the selected tab or if MasterDetail, works with custom pages also
        /// </summary>
        public Task<FreshBasePageModel> SwitchSelectedRootPageModel<T>() where T : FreshBasePageModel
        {
            var currentNavigationService = _currentPageModel.CurrentNavigationService;

            return currentNavigationService.SwitchSelectedRootPageModel<T>();
        }

        /// <summary>
        /// This method is used when you want to switch the selected page, 
        /// </summary>
        public Task<FreshBasePageModel> SwitchSelectedTab<T>() where T : FreshBasePageModel
        {
            return SwitchSelectedRootPageModel<T>();
        }

        /// <summary>
        /// This method is used when you want to switch the selected page, 
        /// </summary>
        public Task<FreshBasePageModel> SwitchSelectedMaster<T>() where T : FreshBasePageModel
        {
            return SwitchSelectedRootPageModel<T>();
        }

        public async Task<FreshNavigationContainer> PushPageModelWithNewNavigation<T> (object data, bool animate = true) where T : FreshBasePageModel
        {
            var page = FreshPageModelResolver.ResolvePageModel<T>(data);
            var naviationContainer = new FreshNavigationContainer(page);
            await PushNewNavigationServiceModal(naviationContainer, page.GetModel(), animate);
            return naviationContainer;
        }

		public void BatchBegin()
		{
			_currentPage.BatchBegin ();
		}

		public void BatchCommit()
		{
			_currentPage.BatchCommit ();
		}

        /// <summary>
        /// Removes current page/pagemodel from navigation
        /// </summary>
        public void RemoveFromNavigation ()
        {
            this._currentPageModel.RaisePageWasPopped ();
            this._currentPage.Navigation.RemovePage (_currentPage);
        }

        /// <summary>
        /// Removes specific page/pagemodel from navigation
        /// </summary>
        public void RemoveFromNavigation<TPageModel> (bool removeAll = false) where TPageModel : FreshBasePageModel =>
            RemoveFromNavigation (typeof(TPageModel), removeAll);

        /// <summary>
        /// Removes specific page/pagemodel from navigation
        /// </summary>
        public void RemoveFromNavigation (Type type, bool removeAll = false)
        {
            foreach (var page in this._currentPage.Navigation.NavigationStack.Reverse().ToList()) 
            {
                if (page.BindingContext?.GetType() == type) 
                {
                    page.GetModel()?.RaisePageWasPopped ();
                    this._currentPage.Navigation.RemovePage (page);
                    if (!removeAll)
                        break;
                }
            }
        }
    }
}


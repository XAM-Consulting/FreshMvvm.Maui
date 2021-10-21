using System;
using Microsoft.Maui.Controls;

namespace FreshMvvm.Maui
{
    public static class PageExtensions
    {
        public static FreshBasePageModel GetModel(this Page page)
        {
            var pageModel = page.BindingContext as FreshBasePageModel;
            return pageModel;
        }

        public static void NotifyAllChildrenPopped(this NavigationPage navigationPage)
        {
            foreach (var page in navigationPage.Navigation.ModalStack)
            {
                var pageModel = page.GetModel();
                if (pageModel != null)
                    pageModel.RaisePageWasPopped();
            }

            foreach (var page in navigationPage.Navigation.NavigationStack)
            {
                var pageModel = page.GetModel();
                if (pageModel != null)
                    pageModel.RaisePageWasPopped();
            }
        }
    }
}


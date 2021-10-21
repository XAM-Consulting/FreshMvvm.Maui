using System;
using Microsoft.Maui.Controls;
using System.ComponentModel;

namespace FreshMvvmApp
{
    public class BasePage : ContentPage
    {
        public BasePage ()
        {            
            ToolbarItems.Add (new ToolbarItem ("Home","", () => {                
                Application.Current.MainPage = new NavigationPage (new LaunchPage ((App)Application.Current));
            }));
        }

        protected override void OnAppearing ()
        {
            base.OnAppearing ();

            var basePageModel = this.BindingContext as FreshMvvm.Maui.FreshBasePageModel;
            if (basePageModel != null) {
                if (basePageModel.IsModalAndHasPreviousNavigationStack ()) {
                    if (ToolbarItems.Count < 2)
                    {
                        var closeModal = new ToolbarItem ("Close Modal", "", () => {                
                            basePageModel.CoreMethods.PopModalNavigationService(); 
                        });

                        ToolbarItems.Add (closeModal);
                    }
                }
            }
        }
    }
}


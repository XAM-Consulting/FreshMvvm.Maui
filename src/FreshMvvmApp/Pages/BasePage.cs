namespace FreshMvvmApp
{
    public class BasePage : ContentPage
    {
        public BasePage ()
        {            
            ToolbarItems.Add (new ToolbarItem ("Home","", () => {
                ((App)Application.Current).GoHome();
            }));
        }

        protected override void OnAppearing ()
        {
            base.OnAppearing ();

            if (BindingContext is FreshMvvm.Maui.FreshBasePageModel basePageModel) {
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


namespace FreshMvvmApp
{
    public partial class App : Application
    {
        private readonly LaunchPage launchPage;

        public App(LaunchPage launchPage)
        {
            InitializeComponent();

            this.launchPage = launchPage;
            GoHome();
        }

        public void GoHome()
        {
            MainPage = new NavigationPage(launchPage);
        }
    }
}

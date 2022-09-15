using FreshMvvm.Maui.Extensions;

namespace FreshMvvmApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<IDatabaseService, DatabaseService>();

            builder.Services.AddSingleton<LaunchPage>();

            builder.Services.AddTransient<ContactListPage>();
            builder.Services.AddTransient<ContactPage>();
            builder.Services.AddTransient<MainMenuPage>();
            builder.Services.AddTransient<ModalPage>();
            builder.Services.AddTransient<QuoteListPage>();
            builder.Services.AddTransient<QuotePage>();

            builder.Services.AddTransient<ContactListPageModel>();
            builder.Services.AddTransient<ContactPageModel>();
            builder.Services.AddTransient<MainMenuPageModel>();
            builder.Services.AddTransient<ModalPageModel>();
            builder.Services.AddTransient<QuoteListPageModel>();
            builder.Services.AddTransient<QuotePageModel>();

            MauiApp mauiApp = builder.Build();

            mauiApp.UseFreshMvvm();

            return mauiApp;
        }
    }
}
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

            builder.Services.Add(ServiceDescriptor.Singleton<IDatabaseService, DatabaseService>());

            builder.Services.Add(ServiceDescriptor.Transient<ContactListPage, ContactListPage>());
            builder.Services.Add(ServiceDescriptor.Transient<ContactPage, ContactPage>());
            builder.Services.Add(ServiceDescriptor.Transient<MainMenuPage, MainMenuPage>());
            builder.Services.Add(ServiceDescriptor.Transient<ModalPage, ModalPage>());
            builder.Services.Add(ServiceDescriptor.Transient<QuoteListPage, QuoteListPage>());
            builder.Services.Add(ServiceDescriptor.Transient<QuotePage, QuotePage>());

            builder.Services.Add(ServiceDescriptor.Transient<ContactListPageModel, ContactListPageModel>());
            builder.Services.Add(ServiceDescriptor.Transient<ContactPageModel, ContactPageModel>());
            builder.Services.Add(ServiceDescriptor.Transient<MainMenuPageModel, MainMenuPageModel>());
            builder.Services.Add(ServiceDescriptor.Transient<ModalPageModel, ModalPageModel>());
            builder.Services.Add(ServiceDescriptor.Transient<QuoteListPageModel, QuoteListPageModel>());
            builder.Services.Add(ServiceDescriptor.Transient<QuotePageModel, QuotePageModel>());

            MauiApp mauiApp = builder.Build();

            mauiApp.UseFreshMvvm();

            return mauiApp;
        }
    }
}
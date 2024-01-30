using GymPassport.WPF.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GymPassport.WPF.State.Accounts;
using GymPassport.WPF.ViewModels;
using GymPassport.WPF.State.Navigators;
using GymPassport.GymPassportAPI.Services.AuthenticationServices;
using GymPassport.GymPassportAPI.Services.GymServices;
using GymPassport.GymPassportAPI.Services.ClientServices;
using Microsoft.Extensions.Options;
using GymPassport.GymPassportAPI.Services.GymEventServices;

namespace GymPassport.WPF.HostBuilders
{
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                // AddServices
                services.AddSingleton<IAuthenticationService, AuthenticationService>();
                services.AddSingleton<IGymService, GymService>();
                services.AddSingleton<IClientService, ClientService>();
                services.AddSingleton<IGymEventService, GymEventService>();
                services.AddSingleton<CloseModalNavigationService>();
                services.AddSingleton<INavigationService>(s => CreateProfileNavigationService(s));

                // Configuración de servicios
                services.AddHttpClient("GymPassportApiHttpClient", (serviceProvider, httpClient) =>
                {
                    AppSettings appSettings = serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value;

                    httpClient.BaseAddress = new Uri(appSettings.BaseAddress);
                });
            });

            return host;
        }

        private static INavigationService CreateProfileNavigationService(IServiceProvider services)
        {
            return new LayoutNavigationService<ProfileViewModel>(
                services.GetRequiredService<IAccountStore>(),
                services.GetRequiredService<NavigationStore>(),
                () => services.GetRequiredService<ProfileViewModel>(),
                () => services.GetRequiredService<NavigationBarViewModel>());
        }
    }
}

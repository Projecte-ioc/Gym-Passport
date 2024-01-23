using GymPassport.WPF.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GymPassport.WPF.State.Accounts;
using GymPassport.WPF.ViewModels;
using GymPassport.WPF.State.Navigators;
using GymPassport.GymPassportAPI.Services.AuthenticationServices;
using GymPassport.GymPassportAPI.Services.ProfileServices;
using GymPassport.GymPassportAPI.Services.GymServices;
using GymPassport.GymPassportAPI.Services.ClientServices;
using GymPassport.GymPassportAPI.ApiConnectors;
using System.ComponentModel.Design.Serialization;
using GymPassport.GymPassportAPI.Services;

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
                services.AddSingleton<IProfileService, ProfileService>();
                services.AddSingleton<IGymService, GymService>();
                services.AddSingleton<IClientService, ClientService>();
                services.AddSingleton<CloseModalNavigationService>();
                services.AddSingleton<INavigationService>(s => CreateProfileNavigationService(s));

                // Configuración de servicios
                services.AddHttpClient();

                // Conectores API
                services.AddTransient<LoginApiConnector>();
                services.AddTransient<GymApiConnector>();
                services.AddTransient<ClientApiConnector>();
                services.AddTransient<GymEventApiConnector>();

                // Servicios API
                services.AddTransient<AuthenticationService>();
                services.AddTransient<GymService>();
                services.AddTransient<ClientService>();
                services.AddTransient<GymEventService>();
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

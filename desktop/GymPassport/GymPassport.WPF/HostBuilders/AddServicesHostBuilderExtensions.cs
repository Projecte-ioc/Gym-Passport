using GymPassport.WPF.Services.AuthenticationServices;
using GymPassport.WPF.Services.ClientServices;
using GymPassport.WPF.Services.GymServices;
using GymPassport.WPF.Services.ProfileServices;
using GymPassport.WPF.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GymPassport.WPF.State.Accounts;
using GymPassport.WPF.Stores;
using GymPassport.WPF.ViewModels;

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

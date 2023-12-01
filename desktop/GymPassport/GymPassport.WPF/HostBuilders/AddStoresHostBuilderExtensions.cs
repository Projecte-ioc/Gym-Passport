using GymPassport.WPF.State;
using GymPassport.WPF.State.Accounts;
using GymPassport.WPF.State.Authenticators;
using GymPassport.WPF.State.Navigators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GymPassport.WPF.HostBuilders
{
    public static class AddStoresHostBuilderExtensions
    {
        public static IHostBuilder AddStores(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                // AddStores
                services.AddSingleton<IAuthenticator, Authenticator>();
                services.AddSingleton<IAccountStore, AccountStore>();

                services.AddSingleton<ClientsStore>();
                services.AddSingleton<SelectedClientStore>();

                services.AddSingleton<NavigationStore>();
                services.AddSingleton<ModalNavigationStore>();
            });

            return host;
        }
    }
}

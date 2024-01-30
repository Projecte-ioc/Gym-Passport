using GymPassport.GymPassportAPI.Services.ClientServices;
using GymPassport.WPF.Services;
using GymPassport.WPF.State.Accounts;
using GymPassport.WPF.State.Authenticators;
using GymPassport.WPF.State.Clients;
using GymPassport.WPF.State.GymEvents;
using GymPassport.WPF.State.Navigators;
using GymPassport.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GymPassport.WPF.HostBuilders
{
    public static class AddViewModelsHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddTransient<ProfileViewModel>(s => new ProfileViewModel(
                        s.GetRequiredService<IAccountStore>(),
                        s.GetRequiredService<IClientService>()));

                services.AddSingleton<LoginViewModel>(s => new LoginViewModel(
                    s.GetRequiredService<IAuthenticator>()));

                services.AddTransient<ClientsViewModel>(CreateClientsViewModel);

                services.AddTransient<ActivitiesViewModel>(s => new ActivitiesViewModel());

                services.AddTransient<GymEventsViewModel>(CreateGymEventsViewModel);

                services.AddTransient<ReservationsViewModel>(s => new ReservationsViewModel());

                services.AddTransient<RoomsViewModel>(s => new RoomsViewModel());

                services.AddSingleton<NavigationBarViewModel>(CreateNavigationBarViewModel);
                services.AddSingleton<MainViewModel>();
            });

            return host;
        }

        private static GymEventsViewModel CreateGymEventsViewModel(IServiceProvider services)
        {
            return GymEventsViewModel.LoadViewModel(
                services.GetRequiredService<GymEventsStore>(),
                services.GetRequiredService<SelectedGymEventStore>(),
                services.GetRequiredService<ModalNavigationStore>()
                );
        }

        private static ClientsViewModel CreateClientsViewModel(IServiceProvider services)
        {
            return ClientsViewModel.LoadViewModel(
                services.GetRequiredService<ClientsStore>(),
                services.GetRequiredService<SelectedClientStore>(),
                services.GetRequiredService<ModalNavigationStore>()
                );
        }

        private static NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider services)
        {
            return new NavigationBarViewModel(services.GetRequiredService<IAccountStore>(),
                            CreateProfileNavigationService(services),
                            CreateClientsNavigationService(services),
                            CreateActivitiesNavigationService(services),
                            CreateGymEventsNavigationService(services),
                            CreateReservationsNavigationService(services),
                            CreateRoomsNavigationService(services));
        }

        private static INavigationService CreateProfileNavigationService(IServiceProvider services)
        {
            return new LayoutNavigationService<ProfileViewModel>(
                services.GetRequiredService<IAccountStore>(),
                services.GetRequiredService<NavigationStore>(),
                () => services.GetRequiredService<ProfileViewModel>(),
                () => services.GetRequiredService<NavigationBarViewModel>());
        }

        private static INavigationService CreateClientsNavigationService(IServiceProvider services)
        {
            return new LayoutNavigationService<ClientsViewModel>(
                services.GetRequiredService<IAccountStore>(),
                services.GetRequiredService<NavigationStore>(),
                () => services.GetRequiredService<ClientsViewModel>(),
                () => services.GetRequiredService<NavigationBarViewModel>());
        }

        private static INavigationService CreateActivitiesNavigationService(IServiceProvider services)
        {
            return new LayoutNavigationService<ActivitiesViewModel>(
                services.GetRequiredService<IAccountStore>(),
                services.GetRequiredService<NavigationStore>(),
                () => services.GetRequiredService<ActivitiesViewModel>(),
                () => services.GetRequiredService<NavigationBarViewModel>());
        }

        private static INavigationService CreateGymEventsNavigationService(IServiceProvider services)
        {
            return new LayoutNavigationService<GymEventsViewModel>(
                services.GetRequiredService<IAccountStore>(),
                services.GetRequiredService<NavigationStore>(),
                () => services.GetRequiredService<GymEventsViewModel>(),
                () => services.GetRequiredService<NavigationBarViewModel>());
        }

        private static INavigationService CreateReservationsNavigationService(IServiceProvider services)
        {
            return new LayoutNavigationService<ReservationsViewModel>(
                services.GetRequiredService<IAccountStore>(),
                services.GetRequiredService<NavigationStore>(),
                () => services.GetRequiredService<ReservationsViewModel>(),
                () => services.GetRequiredService<NavigationBarViewModel>());
        }

        private static INavigationService CreateRoomsNavigationService(IServiceProvider services)
        {
            return new LayoutNavigationService<RoomsViewModel>(
                services.GetRequiredService<IAccountStore>(),
                services.GetRequiredService<NavigationStore>(),
                () => services.GetRequiredService<RoomsViewModel>(),
                () => services.GetRequiredService<NavigationBarViewModel>());
        }
    }
}

using GymPassport.WPF.Services;
using GymPassport.WPF.Services.ClientServices;
using GymPassport.WPF.Services.GymServices;
using GymPassport.WPF.Services.ProfileServices;
using GymPassport.WPF.State.Accounts;
using GymPassport.WPF.State.Authenticators;
using GymPassport.WPF.Stores;
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
                        s.GetRequiredService<IProfileService>()));

                services.AddSingleton<LoginViewModel>(s => new LoginViewModel(
                    s.GetRequiredService<IAuthenticator>()));

                services.AddTransient<ClientsViewModel>(s => new ClientsViewModel(
                    s.GetRequiredService<ClientsStore>(),
                    s.GetRequiredService<IAccountStore>(),
                    s.GetRequiredService<IGymService>(),
                    s.GetRequiredService<IClientService>(),
                    CreateAddClientNavigationService(s),
                    CreateDeleteClientNavigationService(s)));

                services.AddTransient<AddClientViewModel>(s => new AddClientViewModel(
                    s.GetRequiredService<CloseModalNavigationService>(),
                    s.GetRequiredService<ClientsStore>(),
                    s.GetRequiredService<IAccountStore>(),
                    s.GetRequiredService<IClientService>()));

                services.AddTransient<DeleteClientViewModel>(s => new DeleteClientViewModel(
                    s.GetRequiredService<CloseModalNavigationService>(),
                    s.GetRequiredService<ClientsStore>(),
                    s.GetRequiredService<IAccountStore>(),
                    s.GetRequiredService<IClientService>()));

                services.AddTransient<ActivitiesViewModel>(s => new ActivitiesViewModel());

                services.AddTransient<GymEventsViewModel>(s => new GymEventsViewModel());

                services.AddTransient<ReservationsViewModel>(s => new ReservationsViewModel());

                services.AddTransient<RoomsViewModel>(s => new RoomsViewModel());

                services.AddSingleton<NavigationBarViewModel>(CreateNavigationBarViewModel);
                services.AddSingleton<MainViewModel>();
            });

            return host;
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

        // Crea un INavigationService para poder navegar a AddClientView
        private static INavigationService CreateAddClientNavigationService(IServiceProvider services)
        {
            return new ModalNavigationService<AddClientViewModel>(
                services.GetRequiredService<ModalNavigationStore>(),
            () => services.GetRequiredService<AddClientViewModel>());
        }

        private static INavigationService CreateDeleteClientNavigationService(IServiceProvider services)
        {
            return new ModalNavigationService<DeleteClientViewModel>(
                services.GetRequiredService<ModalNavigationStore>(),
                () => services.GetRequiredService<DeleteClientViewModel>());
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

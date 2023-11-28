using GymPassport.WPF.Services;
using GymPassport.WPF.Services.AuthenticationServices;
using GymPassport.WPF.Services.ClientServices;
using GymPassport.WPF.Services.GymServices;
using GymPassport.WPF.Services.ProfileServices;
using GymPassport.WPF.State.Accounts;
using GymPassport.WPF.State.Authenticators;
using GymPassport.WPF.Stores;
using GymPassport.WPF.ViewModels;
using GymPassport.WPF.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace GymPassport.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            // AddStores
            services.AddSingleton<IAuthenticator, Authenticator>();
            services.AddSingleton<IAccountStore, AccountStore>();

            services.AddSingleton<ClientsStore>();
            services.AddSingleton<NavigationStore>();
            services.AddSingleton<ModalNavigationStore>();

            // AddServices
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IProfileService, ProfileService>();
            services.AddSingleton<IGymService, GymService>();
            services.AddSingleton<IClientService, ClientService>();
            services.AddSingleton<CloseModalNavigationService>();

            services.AddSingleton<INavigationService>(s => CreateProfileNavigationService(s));

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

            services.AddSingleton<LoginView>(s => new LoginView()
            {
                DataContext = s.GetRequiredService<LoginViewModel>()
            });

            services.AddSingleton<MainWindow>(s => new MainWindow()
            {
                DataContext = s.GetRequiredService<MainViewModel>()
            });

            _serviceProvider = services.BuildServiceProvider();
        }

        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            var loginView = _serviceProvider.GetRequiredService<LoginView>();
            loginView.Show();
            loginView.IsVisibleChanged += (s, ev) =>
            {
                if (loginView.IsVisible == false && loginView.IsLoaded)
                {
                    INavigationService initialNavigationService = _serviceProvider.GetRequiredService<INavigationService>();
                    initialNavigationService.Navigate();

                    MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                    MainWindow.Show();
                    loginView.Close();
                }
            };
        }

        // Crea un INavigationService para poder navegar a AddClientView
        private INavigationService CreateAddClientNavigationService(IServiceProvider serviceProvider)
        {
            return new ModalNavigationService<AddClientViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
            () => serviceProvider.GetRequiredService<AddClientViewModel>());
        }

        private INavigationService CreateDeleteClientNavigationService(IServiceProvider serviceProvider)
        {
            return new ModalNavigationService<DeleteClientViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<DeleteClientViewModel>());
        }

        private INavigationService CreateProfileNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<ProfileViewModel>(
                serviceProvider.GetRequiredService<IAccountStore>(),
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<ProfileViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>());
        }

        private INavigationService CreateClientsNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<ClientsViewModel>(
                serviceProvider.GetRequiredService<IAccountStore>(),
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<ClientsViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>());
        }

        private INavigationService CreateActivitiesNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<ActivitiesViewModel>(
                serviceProvider.GetRequiredService<IAccountStore>(),
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<ActivitiesViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>());
        }

        private INavigationService CreateGymEventsNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<GymEventsViewModel>(
                serviceProvider.GetRequiredService<IAccountStore>(),
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<GymEventsViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>());
        }

        private INavigationService CreateReservationsNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<ReservationsViewModel>(
                serviceProvider.GetRequiredService<IAccountStore>(),
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<ReservationsViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>());
        }

        private INavigationService CreateRoomsNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<RoomsViewModel>(
                serviceProvider.GetRequiredService<IAccountStore>(),
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<RoomsViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>());
        }

        NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider serviceProvider)
        {
            return new NavigationBarViewModel(serviceProvider.GetRequiredService<IAccountStore>(),
                            CreateProfileNavigationService(serviceProvider),
                            CreateClientsNavigationService(serviceProvider),
                            CreateActivitiesNavigationService(serviceProvider),
                            CreateGymEventsNavigationService(serviceProvider),
                            CreateReservationsNavigationService(serviceProvider),
                            CreateRoomsNavigationService(serviceProvider));
        }
    }

}

using Gym_Passport.Commands;
using Gym_Passport.Services;
using Gym_Passport.State.Accounts;
using System.Windows.Input;

namespace Gym_Passport.ViewModels
{
    public class NavigationBarViewModel : ViewModelBase
    {
        private readonly IAccountStore _accountStore;

        private bool _areAdminButtonsVisible = false;
        public bool AreAdminButtonsVisible
        {
            get
            {
                return _areAdminButtonsVisible;
            }
            set
            {
                _areAdminButtonsVisible = value;
                OnPropertyChanged(nameof(AreAdminButtonsVisible));
            }
        }

        public ICommand ShowProfileViewCommand { get; }
        public ICommand ShowUsersViewCommand { get; }
        public ICommand ShowActiviesViewCommand { get; }
        public ICommand ShowGymEventsViewCommand { get; }
        public ICommand ShowReservationsViewCommand { get; }
        public ICommand ShowRoomsViewCommand { get; }
        public ICommand LogoutCommand { get; }

        public NavigationBarViewModel(IAccountStore accountStore,
            INavigationService accountNavigationService,
            INavigationService usersNavigationService,
            INavigationService activitiesNavigationService,
            INavigationService gymEventsNavigationService,
            INavigationService reservationsNavigationService,
            INavigationService roomsNavigationService)
        {
            _accountStore = accountStore;

            if(accountStore.CurrentAccount.Role == "admin") {
                AreAdminButtonsVisible = true;
            }

            ShowProfileViewCommand = new NavigateCommand(accountNavigationService);
            ShowUsersViewCommand = new NavigateCommand(usersNavigationService);
            ShowActiviesViewCommand = new NavigateCommand(activitiesNavigationService);
            ShowGymEventsViewCommand = new NavigateCommand(gymEventsNavigationService);
            ShowReservationsViewCommand = new NavigateCommand(reservationsNavigationService);
            ShowRoomsViewCommand = new NavigateCommand(roomsNavigationService);
        }
    }
}

using Gym_Passport_Navigation.Commands;
using Gym_Passport_Navigation.Services;
using Gym_Passport_Navigation.State.Accounts;
using System.Windows.Input;

namespace Gym_Passport_Navigation.ViewModels
{
    public class NavigationBarViewModel : ViewModelBase
    {
        private readonly IAccountStore _accountStore;

        private bool _isRoomButtonVisible = false;
        public bool IsRoomButtonVisible
        {
            get
            {
                return _isRoomButtonVisible;
            }
            set
            {
                _isRoomButtonVisible = value;
                OnPropertyChanged(nameof(IsRoomButtonVisible));
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
                IsRoomButtonVisible = true;
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

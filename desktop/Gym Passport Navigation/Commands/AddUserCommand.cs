using Gym_Passport_Navigation.Services;
using Gym_Passport_Navigation.Stores;
using Gym_Passport_Navigation.ViewModels;

namespace Gym_Passport_Navigation.Commands
{
    public class AddUserCommand : CommandBase
    {
        private readonly AddClientViewModel _addUserViewModel;
        private readonly UsersStore _usersStore;
        private readonly INavigationService _navigationService;

        public AddUserCommand(AddClientViewModel addUserViewModel, UsersStore usersStore, INavigationService navigationService)
        {
            _addUserViewModel = addUserViewModel;
            _usersStore = usersStore;
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            _usersStore.AddUser(
                _addUserViewModel.Name,
                _addUserViewModel.Username,
                _addUserViewModel.Role
                );

            _navigationService.Navigate();
        }
    }
}

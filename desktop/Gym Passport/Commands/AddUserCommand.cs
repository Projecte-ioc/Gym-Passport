using Gym_Passport.Services;
using Gym_Passport.Stores;
using Gym_Passport.ViewModels;

namespace Gym_Passport.Commands
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

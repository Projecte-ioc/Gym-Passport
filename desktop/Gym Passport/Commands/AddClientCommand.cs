using Gym_Passport.Commands;
using Gym_Passport.Models;
using Gym_Passport.Services;
using Gym_Passport.State.Accounts;
using Gym_Passport.ViewModels;
using GymPassportPruebasAPI.Services.ClientServices;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Gym_Passport_Navigation.Commands
{
    public class AddClientCommand : AsyncCommandBase
    {
        private readonly AddClientViewModel _addClientViewModel;
        private readonly IAccountStore _accountStore;
        private readonly IClientService _clientService;
        private readonly INavigationService _navigationService;

        public AddClientCommand(AddClientViewModel addClientViewModel, IAccountStore accountStore, IClientService clientService, INavigationService navigationService)
        {
            _addClientViewModel = addClientViewModel;
            _accountStore = accountStore;
            _clientService = clientService;
            _navigationService = navigationService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            await _clientService.InsertClient(_accountStore.CurrentAccount.Token, new
            {
                name = _addClientViewModel.Name,
                pswd_app = _addClientViewModel.Password,
                rol_user = _addClientViewModel.Role,
                user_name = _addClientViewModel.Username
            });
        }
    }
}

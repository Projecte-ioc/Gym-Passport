using Gym_Passport.Commands;
using Gym_Passport.Services;
using Gym_Passport.State.Accounts;
using Gym_Passport_Navigation.ViewModels;
using GymPassportPruebasAPI.Services.ClientServices;
using System.Threading.Tasks;

namespace Gym_Passport_Navigation.Commands
{
    public class DeleteClientCommand : AsyncCommandBase
    {
        private readonly DeleteClientViewModel _deleteClientViewModel;
        private readonly IAccountStore _accountStore;
        private readonly IClientService _clientService;
        private readonly INavigationService _navigationService;

        public DeleteClientCommand(DeleteClientViewModel deleteClientViewModel, IAccountStore accountStore, IClientService clientService, INavigationService navigationService)
        {
            _deleteClientViewModel = deleteClientViewModel;
            _accountStore = accountStore;
            _clientService = clientService;
            _navigationService = navigationService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            await _clientService.DeleteClient(_accountStore.CurrentAccount.Token, "username");
        }
    }
}

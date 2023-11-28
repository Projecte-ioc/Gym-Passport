using GymPassport.WPF.Services.ClientServices;
using GymPassport.WPF.Services;
using GymPassport.WPF.State.Accounts;
using GymPassport.WPF.Stores;
using GymPassport.WPF.ViewModels;

namespace GymPassport.WPF.Commands
{
    public class DeleteClientCommand : AsyncCommandBase
    {
        private readonly DeleteClientViewModel _deleteClientViewModel;
        private readonly ClientsStore _clientsStore;
        private readonly IAccountStore _accountStore;
        private readonly IClientService _clientService;
        private readonly INavigationService _navigationService;

        public DeleteClientCommand(
            DeleteClientViewModel deleteClientViewModel,
            ClientsStore clientsStore,
            IAccountStore accountStore,
            IClientService clientService,
            INavigationService navigationService)
        {
            _deleteClientViewModel = deleteClientViewModel;
            _clientsStore = clientsStore;
            _accountStore = accountStore;
            _clientService = clientService;
            _navigationService = navigationService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            //ClientViewModel client = new ClientViewModel(
            //    _deleteClientViewModel.Name,
            //    _deleteClientViewModel.Role,
            //    _deleteClientViewModel.Username,
            //    _deleteClientViewModel.Password);

            //await _clientService.DeleteClient(_accountStore.CurrentAccount.Token, _deleteClientViewModel.Username);

            //_clientsStore.RemoveClient(client);

            _navigationService.Navigate();

        }
    }
}

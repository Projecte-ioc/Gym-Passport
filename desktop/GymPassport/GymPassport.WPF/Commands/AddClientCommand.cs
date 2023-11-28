using GymPassport.WPF.Services;
using GymPassport.WPF.Services.ClientServices;
using GymPassport.WPF.State.Accounts;
using GymPassport.WPF.Stores;
using GymPassport.WPF.ViewModels;

namespace GymPassport.WPF.Commands
{
    public class AddClientCommand : AsyncCommandBase
    {
        private readonly AddClientViewModel _addClientViewModel;
        private readonly ClientsStore _clientsStore;
        private readonly IAccountStore _accountStore;
        private readonly IClientService _clientService;
        private readonly INavigationService _navigationService;

        public AddClientCommand(AddClientViewModel addClientViewModel,
            ClientsStore clientsStore,
            IAccountStore accountStore,
            IClientService clientService,
            INavigationService navigationService)
        {
            _addClientViewModel = addClientViewModel;
            _clientsStore = clientsStore;
            _accountStore = accountStore;
            _clientService = clientService;
            _navigationService = navigationService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            //ClientViewModel client = new ClientViewModel(
            //    _addClientViewModel.Name,
            //    _addClientViewModel.Role,
            //    _addClientViewModel.Username,
            //    _addClientViewModel.Password);

            await _clientService.InsertClient(_accountStore.CurrentAccount.Token, new
            {
                name = _addClientViewModel.Name,
                pswd_app = _addClientViewModel.Password,
                rol_user = _addClientViewModel.Role,
                user_name = _addClientViewModel.Username
            });

            //_clientsStore.AddClient(client);

            _navigationService.Navigate();
        }
    }
}

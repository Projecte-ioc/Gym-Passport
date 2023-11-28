using GymPassport.WPF.Services.GymServices;
using GymPassport.WPF.State.Accounts;
using GymPassport.WPF.Utils;
using GymPassport.WPF.ViewModels;

namespace GymPassport.WPF.Commands
{
    public class GetAllClientsCommand : AsyncCommandBase
    {
        private readonly ClientsViewModel _clientsViewModel;
        private readonly IGymService _gymService;
        private readonly IAccountStore _accountStore;

        public GetAllClientsCommand(ClientsViewModel clientsViewModel, IGymService gymService, IAccountStore accountStore)
        {
            _clientsViewModel = clientsViewModel;
            _gymService = gymService;
            _accountStore = accountStore;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _clientsViewModel.Clients = await _gymService.GetAllGymClients(_accountStore.CurrentAccount.Token);
            _clientsViewModel.Roles.AddRange(_clientsViewModel.Clients.Select(x => x.Role).Distinct());
        }
    }
}

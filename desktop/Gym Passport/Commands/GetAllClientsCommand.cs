using Gym_Passport.Services.GymServices;
using Gym_Passport.State.Accounts;
using Gym_Passport.ViewModels;
using Gym_Passport_Navigation.Utils;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Gym_Passport.Commands
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

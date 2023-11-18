using Gym_Passport_Navigation.Services.GymServices;
using Gym_Passport_Navigation.Services.ProfileServices;
using Gym_Passport_Navigation.State.Accounts;
using Gym_Passport_Navigation.ViewModels;
using System.Threading.Tasks;

namespace Gym_Passport_Navigation.Commands
{
    public class GetClientsCommand : AsyncCommandBase
    {
        private readonly ClientsViewModel _clientsViewModel;
        private readonly IGymService _gymService;
        private readonly IAccountStore _accountStore;

        public GetClientsCommand(ClientsViewModel clientsViewModel, IGymService gymService, IAccountStore accountStore)
        {
            _clientsViewModel = clientsViewModel;
            _gymService = gymService;
            _accountStore = accountStore;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            //_profileViewModel.UserProfile = await _profileService.GetAllProfileInfo(_accountStore.CurrentAccount.Token);
            _clientsViewModel.Clients = await _gymService.GetAllGymClients(_accountStore.CurrentAccount.Token);
        }
    }
}

using Gym_Passport.Services.GymServices;
using Gym_Passport.Services.ProfileServices;
using Gym_Passport.State.Accounts;
using Gym_Passport.ViewModels;
using System.Threading.Tasks;

namespace Gym_Passport.Commands
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

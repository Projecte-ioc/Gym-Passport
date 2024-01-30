using GymPassport.GymPassportAPI.Services.ClientServices;
using GymPassport.WPF.State.Accounts;
using GymPassport.WPF.ViewModels;

namespace GymPassport.WPF.Commands
{
    public class GetProfileCommand : AsyncCommandBase
    {
        private readonly ProfileViewModel _profileViewModel;
        private readonly IClientService _clientService;
        private readonly IAccountStore _accountStore;

        public GetProfileCommand(ProfileViewModel profileViewModel, IClientService clientService, IAccountStore accountStore)
        {
            _profileViewModel = profileViewModel;
            _clientService = clientService;
            _accountStore = accountStore;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _profileViewModel.UserProfile = await _clientService.GetAllProfileInfo(_accountStore.CurrentAccount.AuthToken);
        }
    }
}

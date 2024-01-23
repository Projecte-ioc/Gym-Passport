using GymPassport.GymPassportAPI.Services.ProfileServices;
using GymPassport.WPF.State.Accounts;
using GymPassport.WPF.ViewModels;

namespace GymPassport.WPF.Commands
{
    public class GetProfileCommand : AsyncCommandBase
    {
        private readonly ProfileViewModel _profileViewModel;
        private readonly IProfileService _profileService;
        private readonly IAccountStore _accountStore;

        public GetProfileCommand(ProfileViewModel profileViewModel, IProfileService profileService, IAccountStore accountStore)
        {
            _profileViewModel = profileViewModel;
            _profileService = profileService;
            _accountStore = accountStore;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _profileViewModel.UserProfile = await _profileService.GetAllProfileInfo(_accountStore.CurrentAccount.AuthToken);
        }
    }
}

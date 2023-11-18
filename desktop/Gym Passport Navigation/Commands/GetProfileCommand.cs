using Gym_Passport_Navigation.Services.ProfileServices;
using Gym_Passport_Navigation.State.Accounts;
using Gym_Passport_Navigation.ViewModels;
using System.Threading.Tasks;

namespace Gym_Passport_Navigation.Commands
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
            _profileViewModel.UserProfile = await _profileService.GetAllProfileInfo(_accountStore.CurrentAccount.Token);
        }
    }
}

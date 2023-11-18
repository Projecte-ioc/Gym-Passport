using Gym_Passport.Services.ProfileServices;
using Gym_Passport.State.Accounts;
using Gym_Passport.ViewModels;
using System.Threading.Tasks;

namespace Gym_Passport.Commands
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

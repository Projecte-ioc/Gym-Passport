using Gym_Passport.Commands;
using Gym_Passport.Models;
using Gym_Passport.Services.ProfileServices;
using Gym_Passport.State.Accounts;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Gym_Passport.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly IAccountStore _accountStore;
        private readonly IProfileService _profileService;
        private UserProfile _userProfile;
        public UserProfile UserProfile
        {
            get
            {
                return _userProfile;
            }
            set
            {
                _userProfile = value;
                OnPropertyChanged(nameof(UserProfile));
            }
        }

        public ICommand GetUserProfileCommand { get; }

        public ProfileViewModel(IAccountStore accountStore, IProfileService profileService)
        {
            _accountStore = accountStore;
            _profileService = profileService;
            GetUserProfileCommand = new GetProfileCommand(this, profileService, accountStore);
            //GetUserProfileAsync().Wait();
            GetUserProfileCommand.Execute(null);
        }

        //public async Task GetUserProfileAsync() 
        //{
        //    _userProfile = await _profileService.GetAllProfileInfo(_accountStore.CurrentAccount.Token);
        //}
    }
}

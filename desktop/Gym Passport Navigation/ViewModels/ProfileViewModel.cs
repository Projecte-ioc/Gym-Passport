using Gym_Passport_Navigation.Commands;
using Gym_Passport_Navigation.Models;
using Gym_Passport_Navigation.Services.ProfileServices;
using Gym_Passport_Navigation.State.Accounts;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Gym_Passport_Navigation.ViewModels
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

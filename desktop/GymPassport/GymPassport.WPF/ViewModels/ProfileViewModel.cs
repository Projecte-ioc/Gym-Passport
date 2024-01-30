using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.Services.ClientServices;
using GymPassport.WPF.Commands;
using GymPassport.WPF.State.Accounts;
using System.Windows.Input;

namespace GymPassport.WPF.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
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

        private string _btnToggleModificationText;
        public string btnToggleModificationText
        {
            get
            {
                return _btnToggleModificationText;
            }
            set
            {
                _btnToggleModificationText = value;
                OnPropertyChanged(nameof(btnToggleModificationText));
            }
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        public ICommand GetUserProfileCommand { get; }
        public ICommand EnableProfileModificationCommand { get; }

        public ProfileViewModel(IAccountStore accountStore, IClientService clientService)
        {
            btnToggleModificationText = "Desbloquejar modificació";
            IsEnabled = false;

            GetUserProfileCommand = new GetProfileCommand(this, clientService, accountStore);
            GetUserProfileCommand.Execute(null);
            EnableProfileModificationCommand = new EnableProfileModificationCommand(this);
        }
    }
}

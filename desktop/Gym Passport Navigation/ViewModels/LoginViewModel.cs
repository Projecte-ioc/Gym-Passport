using Gym_Passport_Navigation.Commands;
using Gym_Passport_Navigation.State.Authenticators;
using System.Windows.Input;

namespace Gym_Passport_Navigation.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        //Campos
        private bool _isViewVisible = true;
        private bool _isProgressBarVisible = false;

        //Propiedades
        private string _username;
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
                OnPropertyChanged(nameof(CanLogin));
            }
        }

        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(CanLogin));
            }
        }

        public bool CanLogin => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        public MessageViewModel ErrorMessageViewModel { get; }

        public string ErrorMessage
        {
            set => ErrorMessageViewModel.Message = value;
        }

        public bool IsViewVisible
        {
            get
            {
                return _isViewVisible;
            }

            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

        public bool IsProgressBarVisible
        {
            get
            {
                return _isProgressBarVisible;
            }
            set
            {
                _isProgressBarVisible = value;
                OnPropertyChanged(nameof(IsProgressBarVisible));
            }
        }


        //-> Comandos
        public ICommand LoginCommand { get; }

        //Constructor
        public LoginViewModel(IAuthenticator authenticator)
        {
            ErrorMessageViewModel = new MessageViewModel();
            LoginCommand = new LoginCommand(this, authenticator);
        }

        public override void Dispose()
        {
            ErrorMessageViewModel.Dispose();

            base.Dispose();
        }
    }
}
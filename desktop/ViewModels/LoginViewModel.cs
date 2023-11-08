using Gym_Passport.Models;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using Gym_Passport.Commands;
using Gym_Passport.Services;

namespace Gym_Passport.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        //Campos
        private string _username;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible = true;
        private UserAccountModel _currentAccount;

        //Propiedades
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
            }
        }
        public SecureString Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
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
        public UserAccountModel CurrentAccount
        {
            get
            {
                return _currentAccount;
            }
            set
            {
                _currentAccount = value;
                OnPropertyChanged(nameof(CurrentAccount));
            }
        }

        //-> Comandos
        public ICommand LoginCommand { get; }

        //Constructor
        public LoginViewModel()
        {
            LoginCommand = new AsyncRelayCommand(Login);
        }

        private async Task<UserAccountModel> Login()
        {
            CurrentAccount = await new AuthenticationService().Login(Username, Password);
            if(CurrentAccount != null)
            {
                IsViewVisible = false;
                return CurrentAccount;
            } else
            {
                ErrorMessage = "* Nom d'usuari o contrasenya invàlid/a.";
                return null;
            }
        }
    }
}

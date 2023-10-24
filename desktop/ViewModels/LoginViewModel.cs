using Gym_Passport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Gym_Passport.Repositories;
using System.Security.Principal;
using System.Threading;

namespace Gym_Passport.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        //Campos
        private string _username;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible = true;

        private IUserRepository userRepository;

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

        //-> Comandos
        public ICommand LoginCommand {  get; }

        //Constructor
        public LoginViewModel()
        {
            userRepository = new UserRepository();
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        }
        /// <summary>
        /// Comando que comprueba si se puede lanzar el comando ExecuteLoginCommand dependiendo de si 
        /// tanto el usuario como la contraseña son correctos, o no lo son.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>true si usuario y contraseña son correctos, false si no lo son.</returns>
        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
                Password == null || Password.Length < 3)
                validData = false;
            else
                 validData = true;
            return validData;
        }

        /// <summary>
        /// Comando que comprueba si el usuario introducido está en la base de datos.
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteLoginCommand(object obj)
        {
            var isValidUser = userRepository.AuthenticateUser(new System.Net.NetworkCredential(Username, Password));
            if(isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(
                    new GenericIdentity(Username), null);
                IsViewVisible = false;
            } else
            {
                ErrorMessage = "* Nombre de usuario o contraseña inválido/a.";
            }
        }
    }
}

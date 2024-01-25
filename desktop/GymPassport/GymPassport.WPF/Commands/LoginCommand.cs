using GymPassport.WPF.Exceptions;
using GymPassport.WPF.State.Authenticators;
using GymPassport.WPF.ViewModels;
using System.ComponentModel;

namespace GymPassport.WPF.Commands
{
    public class LoginCommand : AsyncCommandBase
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly IAuthenticator _authenticator;

        public LoginCommand(LoginViewModel loginViewModel, IAuthenticator authenticator)
        {
            _loginViewModel = loginViewModel;
            _authenticator = authenticator;

            _loginViewModel.PropertyChanged += LoginViewModel_PropertyChanged;
        }

        public override bool CanExecute(object parameter)
        {
            return _loginViewModel.CanLogin && base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _loginViewModel.IsProgressBarVisible = true;
            _loginViewModel.ErrorMessage = string.Empty;

            try
            {
                await _authenticator.Login(_loginViewModel.Username, _loginViewModel.Password);
                _loginViewModel.IsViewVisible = false;
            }
            catch (UserNotFoundException)
            {
                _loginViewModel.ErrorMessage = "El nom d'usuari no existeix.";
            }
            catch (InvalidPasswordException)
            {
                _loginViewModel.ErrorMessage = "Contrasenya incorrecta.";
            }
            catch (Exception)
            {
                _loginViewModel.ErrorMessage = "Ha fallat el login.";
            }
            finally
            {
                _loginViewModel.IsProgressBarVisible = false;
            }
        }

        private void LoginViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LoginViewModel.CanLogin))
            {
                OnCanExecuteChanged();
            }
        }
    }
}

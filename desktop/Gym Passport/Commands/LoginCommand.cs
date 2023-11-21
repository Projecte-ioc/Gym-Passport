using Gym_Passport.Exceptions;
using Gym_Passport.Models;
using Gym_Passport.Services;
using Gym_Passport.State.Authenticators;
using Gym_Passport.ViewModels;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gym_Passport.Commands
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

        //public async Task<Account> Login(string loginUsername, string loginPassword)
        //{
        //    const int COUNT_LIMIT = 10;
        //    Account account = null;

        //    var loginData = new
        //    {
        //        user_name = loginUsername,
        //        pswd_app = loginPassword
        //    };

        //    Console.WriteLine("Iniciando sesión...\n");

        //    for (int i = 0; i < COUNT_LIMIT; i++)
        //    {
        //        try
        //        {
        //            Console.WriteLine("Esperando respuesta HTTP...\n");
        //            string httpResponseContent = await authenticationService.GetHttpResponseContent(loginData);

        //            Console.WriteLine("Esperando token de acceso...\n");
        //            string accessToken = authenticationService.GetAccessToken(httpResponseContent);

        //            Console.WriteLine("Obteniendo objeto Account...\n");
        //            account = authenticationService.GetUserAccount(accessToken);

        //            Console.ForegroundColor = ConsoleColor.Blue;
        //            Console.WriteLine($"Datos del objeto Account:\n\n{account}\n");
        //            Console.ForegroundColor = ConsoleColor.White;

        //            Console.WriteLine("Sesión iniciada con éxito.\n");
        //            break;
        //        }
        //        catch (HttpRequestException ex)
        //        {
        //            Console.BackgroundColor = ConsoleColor.Red;
        //            Console.WriteLine(ex.Message + $"\n\n[Intento de inicio de sesión Nº{i + 1}] Reintentando automáticamente...\n");
        //            Console.BackgroundColor = ConsoleColor.Black;
        //        }
        //    }

        //    return account;
        //}

        private void LoginViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LoginViewModel.CanLogin))
            {
                OnCanExecuteChanged();
            }
        }
    }
}

using GymPassport.WPF.Models;
using GymPassport.WPF.Services.AuthenticationServices;
using GymPassport.WPF.State.Accounts;

namespace GymPassport.WPF.State.Authenticators
{
    public class Authenticator : IAuthenticator
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountStore _accountStore;

        public Authenticator(IAuthenticationService authenticationService, IAccountStore accountStore)
        {
            _authenticationService = authenticationService;
            _accountStore = accountStore;
        }

        public Account CurrentAccount
        {
            get
            {
                return _accountStore.CurrentAccount;
            }
            private set
            {
                _accountStore.CurrentAccount = value;
                StateChanged?.Invoke();
            }
        }

        public bool IsLoggedIn => CurrentAccount != null;

        public event Action StateChanged;

        public async Task Login(string username, string password)
        {
            CurrentAccount = await _authenticationService.Login(username, password);
            //MOCK TEMPORAL

            //Account account = new Account()
            //{
            //    Username = "ivan33usr",
            //    Role = "normal",
            //    GymName = "synergym pepeta",
            //    Name = "Ivan",
            //    Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX25hbWUiOiJpdmFuMzN1c3IiLCJyb2xfdXNlciI6Im5vcm1hbCIsImd5bV9uYW1lIjoic3luZXJneW0gcGVwZXRhIiwibmFtZSI6Ikl2YW4ifQ.W482h6pMc3xOwzEgSKhqYKjundmdt6pslzD_0yG-m9U"
            //};

            //CurrentAccount = account;

            // FIN DEL MOCK TEMPORAL
        }

        public void Logout()
        {
            CurrentAccount = null;
        }
    }
}

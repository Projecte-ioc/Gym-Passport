using GymPassport.Domain.Models;

namespace GymPassport.WPF.State.Authenticators
{
    public interface IAuthenticator
    {
        Account CurrentAccount { get; }
        bool IsLoggedIn { get; }

        event Action StateChanged;

        Task Login(string username, string password);
        void Logout();
    }
}
using GymPassport.WPF.Models;

namespace GymPassport.WPF.Services.AuthenticationServices
{
    public interface IAuthenticationService
    {
        Task<Account> Login(string loginUsername, string loginPassword);
    }
}

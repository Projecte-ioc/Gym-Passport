using GymPassport.Domain.Models;

namespace GymPassport.GymPassportAPI.Services.AuthenticationServices
{
    public interface IAuthenticationService
    {
        Task<Account> Login(string loginUsername, string loginPassword);
    }
}
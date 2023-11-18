using Gym_Passport_Navigation.Models;
using System.Threading.Tasks;

namespace Gym_Passport_Navigation.Services
{
    public interface IAuthenticationService
    {
        Task<string> GetToken(string loginUsername, string loginPassword);
        Task<Account> Login(string loginUsername, string loginPassword);
    }
}
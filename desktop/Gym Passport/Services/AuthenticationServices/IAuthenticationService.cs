using Gym_Passport.Models;
using System.Threading.Tasks;

namespace Gym_Passport.Services
{
    public interface IAuthenticationService
    {
        Task<string> GetToken(string loginUsername, string loginPassword);
        Task<Account> Login(string loginUsername, string loginPassword);
    }
}
using Gym_Passport.Models;
using System.Security;
using System.Threading.Tasks;

namespace Gym_Passport.Services
{
    public interface IAuthenticationService
    {
        Task<string> GetToken(string loginUsername, SecureString loginPassword);
        Task<UserAccountModel> Login(string loginUsername, SecureString loginPassword);
    }
}
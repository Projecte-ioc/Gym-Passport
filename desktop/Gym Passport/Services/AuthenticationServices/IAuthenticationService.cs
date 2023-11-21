using Gym_Passport.Models;
using System.Threading.Tasks;

namespace Gym_Passport.Services
{
    public interface IAuthenticationService
    {
        Task<Account> Login(string loginUsername, string loginPassword);
    }
}
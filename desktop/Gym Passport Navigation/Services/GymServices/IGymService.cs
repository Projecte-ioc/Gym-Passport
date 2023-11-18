using Gym_Passport_Navigation.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Gym_Passport_Navigation.Services.GymServices
{
    public interface IGymService
    {
        Task<ObservableCollection<Client>> GetAllGymClients(string token);
        Task<string> UpdateGym(string accessToken, Gym gym);
    }
}
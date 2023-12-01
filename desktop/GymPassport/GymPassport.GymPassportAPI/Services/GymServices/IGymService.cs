using GymPassport.Domain.Models;
using System.Collections.ObjectModel;

namespace GymPassport.GymPassportAPI.Services.GymServices
{
    public interface IGymService
    {
        Task<ObservableCollection<Client>> GetAllGymClients(string token);
        Task<string> UpdateGym(string accessToken, Gym gym);
    }
}
using GymPassport.Domain.Models;
using System.Collections.ObjectModel;

namespace GymPassport.GymPassportAPI.Services.GymServices
{
    public interface IGymService
    {
        Task<ObservableCollection<Client>> GetAllGymClients(string authToken);
        Task UpdateGym(Gym updatedGym, string authToken);
    }
}
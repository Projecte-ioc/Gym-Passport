using GymPassport.Domain.Models;

namespace GymPassport.GymPassportAPI.Services.GymServices
{
    public interface IGymService
    {
        Task<List<Client>> GetAllGymClients(string authToken);
        Task UpdateGym(string authToken, Gym updatedGym);
    }
}
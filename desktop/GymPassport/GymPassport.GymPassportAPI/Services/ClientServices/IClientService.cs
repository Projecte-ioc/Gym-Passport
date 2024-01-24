using GymPassport.Domain.Models;

namespace GymPassport.GymPassportAPI.Services.ClientServices
{
    public interface IClientService
    {
        Task DeleteClient(string authToken, string deletedClient);
        Task<UserProfile> GetAllProfileInfo(string authToken);
        Task InsertClient(string authToken, Client newClient);
        Task UpdateClient(string authToken, Client updatedClient);
    }
}
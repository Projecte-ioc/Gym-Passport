using GymPassport.Domain.Models;

namespace GymPassport.GymPassportAPI.Services.ClientServices
{
    public interface IClientService
    {
        Task DeleteClient(string accessToken, string deletedClient);
        Task<UserProfile> GetAllProfileInfo(string accessToken);
        Task InsertClient(string accessToken, Client newClient);
        Task UpdateClient(string accessToken, Client updatedClient);
    }
}
using GymPassport.Domain.Models;

namespace GymPassport.GymPassportAPI.Services.ClientServices
{
    public interface IClientService
    {
        Task<bool> DeleteClient(string accessToken, string username);
        Task<UserProfile> GetAllProfileInfo(string accessToken);
        Task<string> GetHttpResponseContent(string accessToken);
        string GetProfileInfoToken(string httpResponseContent);
        UserProfile GetUserProfile(string jwtToken);
        Task<string> InsertClient(string accessToken, Client client);
        Task<string> UpdateClient(string accessToken, Client client);
    }
}
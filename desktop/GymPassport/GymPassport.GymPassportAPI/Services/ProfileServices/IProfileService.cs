using GymPassport.Domain.Models;

namespace GymPassport.GymPassportAPI.Services.ProfileServices
{
    public interface IProfileService
    {
        Task<UserProfile> GetAllProfileInfo(string accessToken);
        Task<string> GetHttpResponseContent(object requestData);
        Task<string> GetHttpResponseContent(string accessToken);
        string GetProfileInfoToken(string httpResponseContent);
        UserProfile GetUserProfile(string jwtToken);
    }
}
using GymPassport.WPF.Models;

namespace GymPassport.WPF.Services.ProfileServices
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

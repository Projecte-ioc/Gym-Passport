using GymPassport.Domain.Models;

namespace GymPassport.GymPassportAPI.Services.ProfileServices
{
    public interface IProfileService
    {
        Task<UserProfile> GetAllProfileInfo(string authToken);
    }
}
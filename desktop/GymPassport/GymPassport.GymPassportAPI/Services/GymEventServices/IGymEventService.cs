using GymPassport.Domain.Models;

namespace GymPassport.GymPassportAPI.Services.GymEventServices
{
    public interface IGymEventService
    {
        Task DeleteGymEvent(string authToken, int deletedGymEvent);
        Task<List<GymEvent>> GetAllGymEvents(string authToken);
        Task InsertGymEvent(string authToken, GymEvent newGymEvent);
        Task UpdateGymEvent(string authToken, GymEvent updatedGymEvent, int id);
    }
}
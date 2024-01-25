using GymPassport.Domain.Models;
using System.Collections.ObjectModel;

namespace GymPassport.GymPassportAPI.Services.GymEventServices
{
    public interface IGymEventService
    {
        Task DeleteGymEvent(string authToken, int id);
        Task<ObservableCollection<GymEvent>> GetAllGymEvents(string authToken);
        Task InsertGymEvent(string authToken, GymEvent newGymEvent);
        Task UpdateGymEvent(string authToken, GymEvent updatedGymEvent, int id);
    }
}
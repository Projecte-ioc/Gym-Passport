using Newtonsoft.Json.Linq;

namespace GymPassport.GymPassportAPI.ApiConnectors
{
    public interface IGymEventApiConnector
    {
        Task DeleteGymEvent(int id, string authToken);
        Task<JObject> GetAllGymEvents(string authToken);
        Task InsertGymEvent(string dataToSend, string authToken);
        Task UpdateGymEvent(int id, string dataToSend, string authToken);
    }
}
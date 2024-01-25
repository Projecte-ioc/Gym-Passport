using Newtonsoft.Json.Linq;

namespace GymPassport.GymPassportAPI.ApiConnectors
{
    public interface IGymApiConnector
    {
        Task<JObject> GetAllGymClients(string authToken);
        Task UpdateGym(string dataToSend, string authToken);
    }
}
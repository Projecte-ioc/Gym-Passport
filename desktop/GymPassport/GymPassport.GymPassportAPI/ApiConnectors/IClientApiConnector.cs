using Newtonsoft.Json.Linq;

namespace GymPassport.GymPassportAPI.ApiConnectors
{
    public interface IClientApiConnector
    {
        Task DeleteClient(string username, string authToken);
        Task<JObject> GetAllProfileInfo(string authToken);
        Task InsertClient(string dataToSend, string authToken);
        Task UpdateClient(string dataToSend, string authToken);
    }
}
using Newtonsoft.Json.Linq;

namespace GymPassport.GymPassportAPI.ApiConnectors
{
    public interface ILoginApiConnector
    {
        Task<JObject> Login(string dataToSend);
        Task Logout(string dataToSend, string authToken);
    }
}
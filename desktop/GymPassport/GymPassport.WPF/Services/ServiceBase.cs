using System.Net.Http;

namespace GymPassport.WPF.Services
{
    public abstract class ServiceBase
    {
        private static HttpClient httpClient;
        private readonly static string host = "10.2.190.11";
        private readonly static string baseURL = $"http://{host}:";

        public static string BaseURL => baseURL;

        public static HttpClient HttpClient { get => httpClient; set => httpClient = value; }
    }
}

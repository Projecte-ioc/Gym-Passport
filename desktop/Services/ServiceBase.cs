using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Gym_Passport.Services
{
    public abstract class ServiceBase
    {
        private static HttpClient httpClient;
        private readonly static string host = "10.2.190.11";
        private readonly static string port = "4000";
        private readonly static string baseURL = $"http://{host}:{port}";

        public static string BaseURL => baseURL;

        public static HttpClient HttpClient { get => httpClient; set => httpClient = value; }

        public async Task<string> PostAsJSONAsync(string url, object requestData)
        {
            using (HttpClient = new HttpClient())
            {
                using var httpResponse = await HttpClient.PostAsJsonAsync(url, requestData);
                switch (httpResponse.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        return await httpResponse.Content.ReadAsStringAsync();
                    default:
                        Console.WriteLine(httpResponse.ToString());
                        return "";
                }
            }
        }
    }
}

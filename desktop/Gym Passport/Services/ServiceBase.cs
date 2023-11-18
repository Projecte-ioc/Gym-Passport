using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Net.Http.Json;
using System.Windows.Forms;

namespace Gym_Passport.Services
{
    public abstract class ServiceBase
    {
        private static HttpClient httpClient;
        private readonly static string host = "10.2.190.11";
        private readonly static string baseURL = $"http://{host}:";

        public static string BaseURL => baseURL;

        public static HttpClient HttpClient { get => httpClient; set => httpClient = value; }

        public async Task<string> PostAsJSONAsync(string url, object requestData)
        {
            string content = "";

            try
            {
                using (HttpClient = new HttpClient())
                {
                    using var httpResponse = await HttpClient.PostAsJsonAsync(url, requestData);
                    switch (httpResponse.StatusCode)
                    {
                        case System.Net.HttpStatusCode.OK:
                            content = await httpResponse.Content.ReadAsStringAsync();
                            break;
                        default:
                            Console.WriteLine(httpResponse.ToString());
                            break;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show("Ha fallat la conexió amb l'API.");
            }
            return content;
        }
    }
}

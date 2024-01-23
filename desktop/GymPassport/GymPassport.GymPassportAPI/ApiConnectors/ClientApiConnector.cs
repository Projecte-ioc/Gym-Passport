using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace GymPassport.GymPassportAPI.ApiConnectors
{
    public class ClientApiConnector
    {
        private const string port = "3000";
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public ClientApiConnector(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _apiUrl = $"http://10.2.190.11:{port}";
        }

        public async Task<JObject> GetAllProfileInfo(string authToken)
        {
            const string path = "/profile_info";
            string route = _apiUrl + path;

            try
            {
                // Agregar el token de autorización al encabezado
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authToken);

                // Envía la petición GET a la API usando el cliente _httpClient inyectado
                HttpResponseMessage response = await _httpClient.GetAsync(route);

                Console.WriteLine(response);

                // Comprueba si la petición ha sido exitosa (Códigos 200 a 299)
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Devuelve la respuesta en formato JObject
                    return JObject.Parse(responseContent);
                }
                else
                {
                    // Gestiona el caso en el que la petición no es exitosa
                    Console.WriteLine($"La petición a la API ha fallado con código: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Gestiona excepciones que puedan ocurrir durante la petición
                Console.WriteLine($"Error: {ex.Message}");
            }
            return null;
        }

        public async Task InsertClient(string dataToSend, string authToken)
        {
            const string path = "/insert_client";
            string route = _apiUrl + path;

            try
            {
                // Agregar el token de autorización al encabezado
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authToken);

                // Crea el contenido que debe ser enviado en la petición POST
                StringContent content = new StringContent(dataToSend, Encoding.UTF8, "application/json");

                // Envía la petición POST a la API usando el cliente _httpClient inyectado
                HttpResponseMessage response = await _httpClient.PostAsync(route, content);

                Console.WriteLine(response);

                // Comprueba si la petición ha sido exitosa (Códigos 200 a 299)
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                }
                else
                {
                    // Gestiona el caso en el que la petición no es exitosa
                    Console.WriteLine($"La petición a la API ha fallado con código: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Gestiona excepciones que puedan ocurrir durante la petición
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task UpdateClient(string dataToSend, string authToken)
        {
            const string path = "/update_data_client";
            string route = _apiUrl + path;

            try
            {
                // Agregar el token de autorización al encabezado
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authToken);

                // Crea el contenido que debe ser enviado en la petición PUT
                StringContent content = new StringContent(dataToSend, Encoding.UTF8, "application/json");

                // Envía la petición PUT a la API usando el cliente _httpClient inyectado
                HttpResponseMessage response = await _httpClient.PutAsync(route, content);

                Console.WriteLine(response);

                // Comprueba si la petición ha sido exitosa (Códigos 200 a 299)
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                }
                else
                {
                    // Gestiona el caso en el que la petición no es exitosa
                    Console.WriteLine($"La petición a la API ha fallado con código: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Gestiona excepciones que puedan ocurrir durante la petición
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task DeleteClient(string username, string authToken)
        {
            const string path = $"/delete_client";
            string query = $"?user_name={username}";
            string route = _apiUrl + path + query;

            try
            {
                // Agregar el token de autorización al encabezado
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authToken);

                // Envía la petición DELETE a la API usando el cliente _httpClient inyectado
                HttpResponseMessage response = await _httpClient.DeleteAsync(route);

                Console.WriteLine(response);

                // Comprueba si la petición ha sido exitosa (Códigos 200 a 299)
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                }
                else
                {
                    // Gestiona el caso en el que la petición no es exitosa
                    Console.WriteLine($"La petición a la API ha fallado con código: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Gestiona excepciones que puedan ocurrir durante la petición
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}

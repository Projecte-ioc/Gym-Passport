using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace GymPassport.GymPassportAPI.ApiConnectors
{
    public class GymEventApiConnector
    {
        private const string port = "6000";
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public GymEventApiConnector(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _apiUrl = $"http://10.2.190.11:{port}";
        }

        public async Task<JObject> GetAllGymEvents(string authToken)
        {
            const string path = "/obtener_eventos";
            string route = _apiUrl + path;

            try
            {
                // Agregar el token de autorización al encabezado
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authToken);

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

        public async Task InsertGymEvent(string dataToSend, string authToken)
        {
            const string path = "/insertar_evento";
            string route = _apiUrl + path;

            try
            {
                // Agregar el token de autorización al encabezado
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authToken);

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

        public async Task UpdateGymEvent(int id, string dataToSend, string authToken)
        {
            const string path = "/modificar_evento";
            string query = $"?event_id={id}";
            string route = _apiUrl + path + query;

            try
            {
                // Agregar el token de autorización al encabezado
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authToken);

                // Crea el contenido que debe ser enviado en la petición PATCH
                StringContent content = new StringContent(dataToSend, Encoding.UTF8, "application/json");

                // Envía la petición PATCH a la API usando el cliente _httpClient inyectado
                HttpResponseMessage response = await _httpClient.PatchAsync(route, content);

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

        public async Task DeleteGymEvent(int id, string authToken)
        {
            const string path = $"/eliminar_evento";
            string query = $"?event_id={id}";
            string route = _apiUrl + path + query;

            try
            {
                // Agregar el token de autorización al encabezado
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authToken);

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

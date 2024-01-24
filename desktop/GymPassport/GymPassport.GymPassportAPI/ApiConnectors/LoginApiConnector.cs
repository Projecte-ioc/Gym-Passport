using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace GymPassport.GymPassportAPI.ApiConnectors
{
    public class LoginApiConnector
    {
        private const string port = "4000";
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public LoginApiConnector(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _apiUrl = $"http://10.2.190.11:{port}";
        }

        public async Task<JObject> Login(string dataToSend)
        {
            const string path = "/login";
            string route = _apiUrl + path;

            // Crea el contenido que debe ser enviado en la petición POST
            StringContent content = new StringContent(dataToSend, Encoding.UTF8, "application/json");

            // Envía la petición POST a la API usando el cliente _httpClient inyectado
            HttpResponseMessage response = await _httpClient.PostAsync(route, content);

            // Lanza una excepción si la petición no ha sido exitosa
            response.EnsureSuccessStatusCode();

            // Almacena el contenido de la respuesta
            string responseContent = await response.Content.ReadAsStringAsync();

            // Devuelve un JObject a partir de la respuesta
            return JObject.Parse(responseContent);
        }

        public async Task Logout(string dataToSend, string authToken)
        {
            const string path = "/logout";
            string route = _apiUrl + path;

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
    }
}

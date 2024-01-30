using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.Helpers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GymPassport.GymPassportAPI.Services.GymServices
{
    /// <summary>
    /// Clase que proporciona servicios relacionados con la gestión de gimnasios mediante interacciones con una API remota.
    /// </summary>
    public class GymService : IGymService
    {
        private const string PORT = "2000";

        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly int _nonceSize;
        private readonly int _tagSize;
        private readonly string _secretKey;

        ///// <summary>
        ///// Constructor de la clase <see cref="GymService"/>.
        ///// </summary>
        ///// <param name="httpClientFactory">Factoría para crear instancias de <see cref="HttpClient"/>.</param>
        ///// <param name="appSettings">Configuraciones de la aplicación proporcionadas mediante <see cref="IOptions{TOptions}"/>.</param>
        //public GymService(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings)
        //{
        //    _httpClient = httpClientFactory.CreateClient("GymPassportApiHttpClient");
        //    _baseAddress = $"{appSettings.Value.BaseAddress}:{PORT}";
        //    _nonceSize = appSettings.Value.NonceSize;
        //    _tagSize = appSettings.Value.TagSize;
        //    _secretKey = appSettings.Value.SecretKey;
        //}
        
        public GymService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("");
            _baseAddress = $"http://10.2.190.11:{PORT}";
            _nonceSize = 12;
            _tagSize = 16;
            _secretKey = "__PROBANDO__probando__";
        }

        /// <summary>
        /// Obtiene de forma asincrónica la lista de clientes asociados a un gimnasio mediante una solicitud GET a la API.
        /// </summary>
        /// <param name="authToken">Token de autorización utilizado para identificar y autenticar al usuario.</param>
        /// <returns>
        /// Una tarea asincrónica que representa la operación de obtención de la lista de clientes.
        /// La tarea devuelve una lista de instancias de la clase Client si la petición es exitosa.
        /// </returns>
        /// <exception cref="HttpRequestException">Se lanza en caso de que la petición a la API no sea exitosa.</exception>
        public async Task<List<Client>> GetAllGymClients(string authToken)
        {
            const string path = "/consultar_clientes_gym";
            Uri route = new Uri(_baseAddress + path);

            // Agregar el token de autorización al encabezado
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authToken);

            // Envía la petición GET a la API
            HttpResponseMessage response = await _httpClient.GetAsync(route);

            // Comprueba si la petición ha sido exitosa (Códigos 200 a 299)
            if (response.IsSuccessStatusCode)
            {
                // Lee de forma asíncrona el contenido de la respuesta HTTP y almacena el resultado como una cadena
                string responseContent = await response.Content.ReadAsStringAsync();

                // Desencripta y decodifica el JWT recibido desde la API
                string jsonToken = ApiUtils.DecryptAndDecodeJwt(responseContent, _secretKey, _nonceSize, _tagSize);

                // Pasa el string con el contenido del JWT a JObject
                JObject jObject = JObject.Parse(jsonToken);

                // Mapear los claims del JWT a una lista de instancias de la clase Client
                List<Client> clients = JsonConvert.DeserializeObject<List<Client>>(jObject["results"].ToString());

                // Devuelve la lista de clientes
                return clients;
            }
            else
            {
                // Gestiona el caso en el que la petición no es exitosa
                throw new HttpRequestException($"La petición a la API ha fallado con código: {response.StatusCode}. Contenido: {response}");
            }
        }

        /// <summary>
        /// Actualiza los datos de un gimnasio mediante una solicitud PATCH a la API de autenticación.
        /// </summary>
        /// <param name="updatedGym">Instancia de la clase Gym con los datos actualizados del gimnasio.</param>
        /// <param name="authToken">Token de autorización utilizado para identificar y autenticar al usuario.</param>
        /// <returns>Una tarea asincrónica que representa la operación de actualización del gimnasio.</returns>
        /// <exception cref="HttpRequestException">Se lanza en caso de que la petición a la API no sea exitosa.</exception>
        public async Task UpdateGym(string authToken, Gym updatedGym)
        {
            const string path = "/update_gym";
            Uri route = new Uri(_baseAddress + path);

            // Crea un JObject con los datos del gimnasio actualizado
            JObject updatedGymAsJObject = JObject.FromObject(updatedGym);

            // Crea un string que contiene un JWT firmado y encriptado
            StringContent content = ApiUtils.EncodeAndEncryptJwt(updatedGymAsJObject, _secretKey, _nonceSize, _tagSize);

            // Agregar el token de autorización al encabezado
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authToken);

            // Envía la petición PATCH a la API usando el cliente _httpClient inyectado
            HttpResponseMessage response = await _httpClient.PatchAsync(route, content);

            // Comprueba si la petición ha sido exitosa (Códigos 200 a 299)
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
            }
            else
            {
                // Gestiona el caso en el que la petición no es exitosa
                throw new HttpRequestException($"La petición a la API ha fallado con código: {response.StatusCode}. Contenido: {response}");
            }
        }
    }
}
using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.Helpers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GymPassport.GymPassportAPI.Services.ClientServices
{
    /// <summary>
    /// Clase que proporciona servicios relacionados con la gestión de clientes de un gimnasio mediante interacciones con una API remota.
    /// </summary>
    public class ClientService : IClientService
    {
        private const string PORT = "3000";

        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly int _nonceSize;
        private readonly int _tagSize;
        private readonly string _secretKey;

        ///// <summary>
        ///// Constructor de la clase <see cref="ClientService"/>.
        ///// </summary>
        ///// <param name="httpClientFactory">Factoría para crear instancias de <see cref="HttpClient"/>.</param>
        ///// <param name="appSettings">Configuraciones de la aplicación proporcionadas mediante <see cref="IOptions{TOptions}"/>.</param>
        //public ClientService(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings)
        //{
        //    _httpClient = httpClientFactory.CreateClient("GymPassportApiHttpClient");
        //    _baseAddress = $"{appSettings.Value.BaseAddress}:{PORT}";
        //    _nonceSize = appSettings.Value.NonceSize;
        //    _tagSize = appSettings.Value.TagSize;
        //    _secretKey = appSettings.Value.SecretKey;
        //}

        public ClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("");
            _baseAddress = $"http://10.2.190.11:{PORT}";
            _nonceSize = 12;
            _tagSize = 16;
            _secretKey = "__PROBANDO__probando__";
        }

        /// <summary>
        /// Obtiene de forma asincrónica la información del perfil de usuario mediante una solicitud GET a la API.
        /// </summary>
        /// <param name="accessToken">Token de acceso utilizado para identificar y autenticar al usuario.</param>
        /// <returns>
        /// Una tarea asincrónica que representa la operación de obtención de la información del perfil de usuario.
        /// La tarea devuelve una instancia de la clase UserProfile si la petición es exitosa.
        /// </returns>
        /// <exception cref="HttpRequestException">Se lanza en caso de que la petición a la API no sea exitosa.</exception>
        public async Task<UserProfile> GetAllProfileInfo(string accessToken)
        {
            const string path = "/profile_info";
            Uri route = new Uri(_baseAddress + path);

            // Agregar el token de autorización al encabezado
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", accessToken);

            // Envía la petición GET a la API
            HttpResponseMessage response = await _httpClient.GetAsync(route);

            // Comprueba si la petición ha sido exitosa (Códigos 200 a 299)
            if (response.IsSuccessStatusCode)
            {
                // Lee de forma asíncrona el contenido de la respuesta HTTP y almacena el resultado como una cadena
                string responseContent = await response.Content.ReadAsStringAsync();

                // Desencripta y decodifica el JWT recibido desde la API
                string jsonToken = ApiUtils.DecryptAndDecodeJwt(responseContent, _secretKey, _nonceSize, _tagSize);

                // Mapear los claims del JWT a una lista de instancias de la clase Client
                UserProfile userProfile = JsonConvert.DeserializeObject<UserProfile>(jsonToken);

                // Devuelve el perfil del usuario logueado
                return userProfile;
            }
            else
            {
                // Gestiona el caso en el que la petición no es exitosa
                throw new HttpRequestException($"La petición a la API ha fallado con código: {response.StatusCode}. Contenido: {response}");
            }
        }

        /// <summary>
        /// Inserta un nuevo cliente mediante una solicitud POST a la API.
        /// </summary>
        /// <param name="newClient">Instancia de la clase Client con los datos del nuevo cliente.</param>
        /// <param name="accessToken">Token de autorización utilizado para identificar y autenticar al usuario.</param>
        /// <returns>Una tarea asincrónica que representa la operación de inserción del cliente.</returns>
        /// <exception cref="HttpRequestException">Se lanza en caso de que la petición a la API no sea exitosa.</exception>
        public async Task InsertClient(string accessToken, Client newClient)
        {
            const string path = "/insert_client";
            Uri route = new Uri(_baseAddress + path);

            // Crea un JObject con los datos del nuevo cliente
            JObject newClientAsJObject = JObject.FromObject(newClient);

            // Crea un string que contiene un JWT firmado y encriptado
            StringContent content = ApiUtils.EncodeAndEncryptJwt(newClientAsJObject, _secretKey, _nonceSize, _tagSize);

            // Agregar el token de autorización al encabezado
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", accessToken);

            // Envía la petición POST a la API
            HttpResponseMessage response = await _httpClient.PostAsync(route, content);

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

        /// <summary>
        /// Actualiza los datos de un cliente mediante una solicitud PUT a la API.
        /// </summary>
        /// <param name="updatedClient">Instancia de la clase Client con los datos actualizados del cliente.</param>
        /// <param name="accessToken">Token de autorización utilizado para identificar y autenticar al usuario.</param>
        /// <returns>Una tarea asincrónica que representa la operación de actualización del cliente.</returns>
        /// <exception cref="HttpRequestException">Se lanza en caso de que la petición a la API no sea exitosa.</exception>
        public async Task UpdateClient(string accessToken, Client updatedClient)
        {
            const string path = "/update_data_client";
            Uri route = new Uri(_baseAddress + path);

            // Crea un JObject con los datos del cliente actualizado
            JObject updatedClientAsJObject = JObject.FromObject(updatedClient);

            // Crea un string que contiene un JWT firmado y encriptado
            StringContent content = ApiUtils.EncodeAndEncryptJwt(updatedClientAsJObject, _secretKey, _nonceSize, _tagSize);

            // Agregar el token de autorización al encabezado
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", accessToken);

            // Envía la petición PUT a la API
            HttpResponseMessage response = await _httpClient.PutAsync(route, content);

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

        /// <summary>
        /// Elimina un cliente mediante una solicitud DELETE a la API.
        /// </summary>
        /// <param name="deletedClient">Nombre de usuario del cliente a eliminar.</param>
        /// <param name="accessToken">Token de autorización utilizado para identificar y autenticar al usuario.</param>
        /// <returns>Una tarea asincrónica que representa la operación de eliminación del cliente.</returns>
        /// <exception cref="HttpRequestException">Se lanza en caso de que la petición a la API no sea exitosa.</exception>
        public async Task DeleteClient(string accessToken, string deletedClient)
        {
            const string path = $"/delete_client";
            string query = $"?user_name={deletedClient}";
            Uri route = new Uri(_baseAddress + path + query);

            // Agregar el token de autorización al encabezado
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", accessToken);

            // Envía la petición DELETE a la API
            HttpResponseMessage response = await _httpClient.DeleteAsync(route);

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
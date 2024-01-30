using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.Helpers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace GymPassport.GymPassportAPI.Services.GymEventServices
{
    /// <summary>
    /// Clase que proporciona servicios relacionados con la gestión de eventos de gimnasios mediante interacciones con una API remota.
    /// </summary>
    public class GymEventService : IGymEventService
    {
        private const string PORT = "6000";

        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly int _nonceSize;
        private readonly int _tagSize;
        private readonly string _secretKey;

        ///// <summary>
        ///// Constructor de la clase <see cref="GymEventService"/>.
        ///// </summary>
        ///// <param name="httpClientFactory">Factoría para crear instancias de <see cref="HttpClient"/>.</param>
        ///// <param name="appSettings">Configuraciones de la aplicación proporcionadas mediante <see cref="IOptions{TOptions}"/>.</param>
        //public GymEventService(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings)
        //{
        //    _httpClient = httpClientFactory.CreateClient("GymPassportApiHttpClient");
        //    _baseAddress = $"{appSettings.Value.BaseAddress}:{PORT}";
        //    _nonceSize = appSettings.Value.NonceSize;
        //    _tagSize = appSettings.Value.TagSize;
        //    _secretKey = appSettings.Value.SecretKey;
        //}

        public GymEventService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("");
            _baseAddress = $"http://10.2.190.11:{PORT}";
            _nonceSize = 12;
            _tagSize = 16;
            _secretKey = "__PROBANDO__probando__";
        }

        /// <summary>
        /// Obtiene de forma asincrónica la lista de eventos de gimnasio mediante una solicitud GET a la API.
        /// </summary>
        /// <param name="authToken">Token de autorización utilizado para identificar y autenticar al usuario.</param>
        /// <returns>
        /// Una tarea asincrónica que representa la operación de obtención de la lista de eventos.
        /// La tarea devuelve una lista de instancias de la clase GymEvent si la petición es exitosa.
        /// </returns>
        /// <exception cref="HttpRequestException">Se lanza en caso de que la petición a la API no sea exitosa.</exception>
        public async Task<List<GymEvent>> GetAllGymEvents(string authToken)
        {
            const string path = "/obtener_eventos";
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

                // Mapear los claims del JWT a una lista de instancias de la clase GymEvent
                List<GymEvent> gymEvents = JsonConvert.DeserializeObject<List<GymEvent>>(jObject["results"].ToString());

                return gymEvents;
            }
            else
            {
                // Gestiona el caso en el que la petición no es exitosa
                throw new HttpRequestException($"La petición a la API ha fallado con código: {response.StatusCode}. Contenido: {response}");
            }
        }

        /// <summary>
        /// Inserta un nuevo evento de gimnasio mediante una solicitud POST a la API.
        /// </summary>
        /// <param name="newGymEvent">Instancia de la clase GymEvent que representa el nuevo evento.</param>
        /// <param name="authToken">Token de autorización utilizado para identificar y autenticar al usuario.</param>
        /// <returns>Una tarea asincrónica que representa la operación de inserción del evento.</returns>
        /// <exception cref="HttpRequestException">Se lanza en caso de que la petición a la API no sea exitosa.</exception>
        public async Task InsertGymEvent(string authToken, GymEvent newGymEvent)
        {
            const string path = "/insertar_evento";
            Uri route = new Uri(_baseAddress + path);

            // Crea un JObject con los datos del evento
            JObject newGymEventAsJObject = JObject.FromObject(newGymEvent);

            // Crea un string que contiene un JWT firmado y encriptado
            StringContent content = ApiUtils.EncodeAndEncryptJwt(newGymEventAsJObject, _secretKey, _nonceSize, _tagSize);

            // Agregar el token de autorización al encabezado
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authToken);

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
        /// Actualiza un evento de gimnasio mediante una solicitud PATCH a la API.
        /// </summary>
        /// <param name="id">Identificador del evento de gimnasio a actualizar.</param>
        /// <param name="updatedGymEvent">Instancia de la clase GymEvent con los datos actualizados.</param>
        /// <param name="authToken">Token de autorización utilizado para identificar y autenticar al usuario.</param>
        /// <returns>Una tarea asincrónica que representa la operación de actualización del evento.</returns>
        /// <exception cref="HttpRequestException">Se lanza en caso de que la petición a la API no sea exitosa.</exception>
        public async Task UpdateGymEvent(string authToken, GymEvent updatedGymEvent, int id)
        {
            const string path = "/modificar_evento";
            string query = $"?event_id={id}";
            Uri route = new Uri(_baseAddress + path + query);

            // Crea un JObject con los datos del evento actualizado
            JObject updatedGymEventAsJObject = JObject.FromObject(updatedGymEvent);

            // Crea un string que contiene un JWT firmado y encriptado
            StringContent content = ApiUtils.EncodeAndEncryptJwt(updatedGymEventAsJObject, _secretKey, _nonceSize, _tagSize);

            // Agregar el token de autorización al encabezado
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authToken);

            // Envía la petición PATCH a la API
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

        /// <summary>
        /// Elimina un evento de gimnasio mediante una solicitud DELETE a la API.
        /// </summary>
        /// <param name="deletedGymEvent">Identificador del evento de gimnasio a eliminar.</param>
        /// <param name="authToken">Token de autorización utilizado para identificar y autenticar al usuario.</param>
        /// <returns>Una tarea asincrónica que representa la operación de eliminación del evento.</returns>
        /// <exception cref="HttpRequestException">Se lanza en caso de que la petición a la API no sea exitosa.</exception>
        public async Task DeleteGymEvent(string authToken, int deletedGymEvent)
        {
            const string path = $"/eliminar_evento";
            string query = $"?event_id={deletedGymEvent}";
            Uri route = new Uri(_baseAddress + path + query);

            // Agregar el token de autorización al encabezado
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authToken);

            // Envía la petición DELETE a la API usando el cliente _httpClient inyectado
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
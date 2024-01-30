using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.Helpers;
using Jose;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace GymPassport.GymPassportAPI.Services.AuthenticationServices
{
    /// <summary>
    /// Clase que proporciona servicios relacionados con la autenticación de usuarios mediante interacciones con una API remota.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private const string PORT = "4000";

        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly int _nonceSize;
        private readonly int _tagSize;
        private readonly string _secretKey;

        ///// <summary>
        ///// Constructor de la clase <see cref="AuthenticationService"/>.
        ///// </summary>
        ///// <param name="httpClientFactory">Factoría para crear instancias de <see cref="HttpClient"/>.</param>
        ///// <param name="appSettings">Configuraciones de la aplicación proporcionadas mediante <see cref="IOptions{TOptions}"/>.</param>
        //public AuthenticationService(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings)
        //{
        //    _httpClient = httpClientFactory.CreateClient("GymPassportApiHttpClient");
        //    _baseAddress = $"{appSettings.Value.BaseAddress}:{PORT}";
        //    _nonceSize = appSettings.Value.NonceSize;
        //    _tagSize = appSettings.Value.TagSize;
        //    _secretKey = appSettings.Value.SecretKey;
        //}

        public AuthenticationService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("");
            _baseAddress = $"http://10.2.190.11:{PORT}";
            _nonceSize = 12;
            _tagSize = 16;
            _secretKey = "__PROBANDO__probando__";
        }

        /// <summary>
        /// Autentica un usuario mediante una solicitud asincrónica a la API de autenticación.
        /// </summary>
        /// <param name="loginData">Datos de inicio de sesión del usuario.</param>
        /// <returns>Una tarea asincrónica que representa la operación de autenticación y devuelve una instancia de la clase Account si la autenticación es exitosa.</returns>
        /// <exception cref="HttpRequestException">Se lanza en caso de que la petición a la API no sea exitosa.</exception>
        public async Task<Account> Login(LoginData loginData)
        {
            const string path = "/login";
            Uri route = new Uri(_baseAddress + path);

            // Crea un JObject con los datos del login
            JObject loginDataObject = JObject.FromObject(loginData);

            // Crea un string que contiene un JWT firmado y encriptado
            StringContent content = ApiUtils.EncodeAndEncryptJwt(loginDataObject, _secretKey, _nonceSize, _tagSize);

            // Envía la petición POST a la API
            HttpResponseMessage response = await _httpClient.PostAsync(route, content);

            // Comprueba si la petición ha sido exitosa (Códigos 200 a 299)
            if (response.IsSuccessStatusCode)
            {
                // Lee de forma asíncrona el contenido de la respuesta HTTP y almacena el resultado como una cadena
                string responseContent = await response.Content.ReadAsStringAsync();

                // Devuelve la respuesta en formato JObject
                JObject jObjectResponseContent = JObject.Parse(responseContent);

                // Almacena el JWT encriptado recibido
                string encryptedResponse = jObjectResponseContent["jwe"]!.ToString();

                // Desencripta el JWT recibido
                string decryptedResponse = AesGcmUtils.Decrypt(encryptedResponse, _secretKey, _nonceSize, _tagSize);

                // Decodificar el JWT recibido
                string jsonToken = JWT.Decode(decryptedResponse, Encoding.UTF8.GetBytes(_secretKey), JwsAlgorithm.HS256);

                // Mapear los claims del JWT a una instancia de la clase Account
                Account? account = JsonConvert.DeserializeObject<Account>(jsonToken);

                // Guarda el token de autorización para futuras peticiones HTTP en la cuenta logueada
                account!.AuthToken = encryptedResponse;

                // Devuelve la instancia de la clase Account
                return account;
            }
            else
            {
                // Gestiona el caso en el que la petición no es exitosa
                throw new HttpRequestException($"La petición a la API ha fallado con código: {response.StatusCode}. Contenido: {response}");
            }
        }

        /// <summary>
        /// Realiza la operación de cierre de sesión (logout) mediante una solicitud asincrónica a la API de autenticación.
        /// </summary>
        /// <param name="authToken">Token de autorización utilizado para identificar y autenticar al usuario.</param>
        /// <returns>Una tarea asincrónica que representa la operación de cierre de sesión.</returns>
        /// <exception cref="HttpRequestException">Se lanza en caso de que la petición a la API no sea exitosa.</exception>
        public async Task Logout(string authToken)
        {
            const string path = "/logout";
            Uri route = new Uri(_baseAddress + path);

            // Crea un JObject con los datos del logout
            JObject logoutDataObject = new JObject { { "log", "0" } };

            // Crea un string que contiene un JWT firmado y encriptado
            StringContent content = ApiUtils.EncodeAndEncryptJwt(logoutDataObject, _secretKey, _nonceSize, _tagSize);

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
    }
}
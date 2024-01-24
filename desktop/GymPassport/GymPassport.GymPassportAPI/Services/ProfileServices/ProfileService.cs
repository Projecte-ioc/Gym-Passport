using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.ApiConnectors;
using GymPassport.GymPassportAPI.Helpers;
using GymPassport.GymPassportAPI.Services.AuthenticationServices;
using Jose;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace GymPassport.GymPassportAPI.Services.ProfileServices
{
    public class ProfileService : IProfileService
    {
        private readonly AppSettings _appSettings;
        private readonly ClientApiConnector _clientApiConnector;

        public ProfileService(IOptions<AppSettings> appSettings, ClientApiConnector clientApiConnector)
        {
            _appSettings = appSettings.Value;
            _clientApiConnector = clientApiConnector;
        }

        public async Task<UserProfile> GetAllProfileInfo(string authToken)
        {
            // Envía el token de autorización a la API y espera su respuesta
            JObject apiResponse = await _clientApiConnector.GetAllProfileInfo(authToken);

            // Almacena el JWT encriptado recibido
            string encryptedResponse = apiResponse["jwe"].ToString();

            // Desencripta el JWT recibido
            string decryptedResponse = AesGcmUtils.DecryptWithAESGCM(encryptedResponse, _appSettings.SecretKey);

            // Decodifica el JWT recibido
            JObject jsonToken = JObject.Parse(JWT.Decode(decryptedResponse, Encoding.UTF8.GetBytes(_appSettings.SecretKey), JwsAlgorithm.HS256));

            // Mapear los claims del JWT a una lista de instancias de la clase Client
            UserProfile userProfile = JsonConvert.DeserializeObject<UserProfile>(jsonToken.ToString());

            return userProfile;
        }
    }
}

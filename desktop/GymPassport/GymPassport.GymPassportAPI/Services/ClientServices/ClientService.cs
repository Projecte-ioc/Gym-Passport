using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.ApiConnectors;
using GymPassport.GymPassportAPI.Helpers;
using Jose;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace GymPassport.GymPassportAPI.Services.ClientServices
{
    public class ClientService : IClientService
    {
        // SECRET KEY ALMACENADA TEMPORALMENTE
        string secretKey = "__PROBANDO__probando__";
        // SECRET KEY ALMACENADA TEMPORALMENTE
        private readonly IClientApiConnector _clientApiConnector;

        public ClientService(IClientApiConnector clientApiConnector)
        {
            _clientApiConnector = clientApiConnector;
        }

        public async Task<UserProfile> GetAllProfileInfo(string authToken)
        {
            // Envía el token de autorización a la API y espera su respuesta
            JObject apiResponse = await _clientApiConnector.GetAllProfileInfo(authToken);

            // Almacena el JWT encriptado recibido
            string encryptedResponse = apiResponse["jwe"].ToString();

            // Desencripta el JWT recibido
            string decryptedResponse = AesGcmUtils.DecryptWithAESGCM(encryptedResponse, secretKey);

            // Decodifica el JWT recibido
            JObject jsonToken = JObject.Parse(JWT.Decode(decryptedResponse, Encoding.UTF8.GetBytes(secretKey), JwsAlgorithm.HS256));

            // Mapear los claims del JWT a una lista de instancias de la clase Client
            UserProfile userProfile = JsonConvert.DeserializeObject<UserProfile>(jsonToken.ToString());

            return userProfile;
        }

        public async Task InsertClient(string authToken, Client newClient)
        {
            // Crea un JObject con los datos del nuevo cliente
            JObject newClientAsJObject = JObject.FromObject(newClient);

            // Crea un JObject que contiene un JWT firmado y encriptado
            string encryptedJwt = ApiUtils.CreateSignedAndEncryptedJwt(newClientAsJObject, secretKey);

            // Envía el token de autorización a la API y espera su respuesta
            await _clientApiConnector.InsertClient(encryptedJwt, authToken);
        }

        public async Task UpdateClient(string authToken, Client updatedClient)
        {
            // Crea un JObject con los datos del cliente actualizado
            JObject updatedClientAsJObject = JObject.FromObject(updatedClient);

            // Crea un JObject que contiene un JWT firmado y encriptado
            string encryptedJwt = ApiUtils.CreateSignedAndEncryptedJwt(updatedClientAsJObject, secretKey);

            // Envía el token de autorización a la API y espera su respuesta
            await _clientApiConnector.UpdateClient(encryptedJwt, authToken);
        }

        public async Task DeleteClient(string authToken, string deletedClient)
        {
            // Envía el token de autorización a la API y espera su respuesta
            await _clientApiConnector.DeleteClient(deletedClient, authToken);
        }
    }
}

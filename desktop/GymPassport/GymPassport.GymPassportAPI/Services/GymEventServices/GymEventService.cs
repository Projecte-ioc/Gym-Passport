using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.ApiConnectors;
using GymPassport.GymPassportAPI.Helpers;
using Jose;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace GymPassport.GymPassportAPI.Services.GymEventServices
{
    public class GymEventService : IGymEventService
    {
        // SECRET KEY ALMACENADA TEMPORALMENTE
        string secretKey = "__PROBANDO__probando__";
        // SECRET KEY ALMACENADA TEMPORALMENTE
        private readonly IGymEventApiConnector _gymEventApiConnector;

        public GymEventService(IGymEventApiConnector gymEventApiConnector)
        {
            _gymEventApiConnector = gymEventApiConnector;
        }

        public async Task<List<GymEvent>> GetAllGymEvents(string authToken)
        {
            // Envía el token de autorización a la API y espera su respuesta
            JObject apiResponse = await _gymEventApiConnector.GetAllGymEvents(authToken);

            // Almacena el JWT encriptado recibido
            string encryptedResponse = apiResponse["jwe"].ToString();

            // Desencripta el JWT recibido
            string decryptedResponse = AesGcmUtils.DecryptWithAESGCM(encryptedResponse, secretKey);

            // Decodificar el JWT recibido
            JObject jsonToken = JObject.Parse(JWT.Decode(decryptedResponse, Encoding.UTF8.GetBytes(secretKey), JwsAlgorithm.HS256));

            // Mapear los claims del JWT a una lista de instancias de la clase Client
            List<GymEvent> gymEvents = JsonConvert.DeserializeObject<List<GymEvent>>(jsonToken["results"].ToString());

            return gymEvents;
        }

        public async Task InsertGymEvent(string authToken, GymEvent newGymEvent)
        {
            // Crea un JObject con los datos del evento
            JObject newGymEventAsJObject = JObject.FromObject(newGymEvent);

            // Crea un JObject que contiene un JWT firmado y encriptado
            string encryptedJwt = ApiUtils.CreateSignedAndEncryptedJwt(newGymEventAsJObject, secretKey);

            // Envía el token de autorización a la API y espera su respuesta
            await _gymEventApiConnector.InsertGymEvent(encryptedJwt, authToken);
        }

        public async Task UpdateGymEvent(string authToken, GymEvent updatedGymEvent, int id)
        {
            // Crea un JObject con los datos del evento actualizado
            JObject updatedGymEventAsJObject = JObject.FromObject(updatedGymEvent);

            // Crea un JObject que contiene un JWT firmado y encriptado
            string encryptedJwt = ApiUtils.CreateSignedAndEncryptedJwt(updatedGymEventAsJObject, secretKey);

            // Envía el token de autorización a la API y espera su respuesta
            await _gymEventApiConnector.UpdateGymEvent(id, encryptedJwt, authToken);
        }

        public async Task DeleteGymEvent(string authToken, int id)
        {
            // Envía el token de autorización a la API y espera su respuesta
            await _gymEventApiConnector.DeleteGymEvent(id, authToken);
        }
    }
}

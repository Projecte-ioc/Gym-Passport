using GymPassport.GymPassportAPI.ApiConnectors;
using GymPassport.GymPassportAPI.Helpers;
using Jose;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using GymPassport.Domain.Models;
using System.Collections.ObjectModel;

namespace GymPassport.GymPassportAPI.Services
{
    public class GymEventService
    {
        // SECRET KEY ALMACENADA TEMPORALMENTE
        string secretKey = "__PROBANDO__probando__";
        // SECRET KEY ALMACENADA TEMPORALMENTE
        private readonly GymEventApiConnector _gymEventApiConnector;

        public GymEventService(GymEventApiConnector gymEventApiConnector)
        {
            _gymEventApiConnector = gymEventApiConnector;
        }

        public async Task<ObservableCollection<GymEvent>> GetAllGymEvents(string authToken)
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
            ObservableCollection<GymEvent> gymEvents = JsonConvert.DeserializeObject<ObservableCollection<GymEvent>>(jsonToken["results"].ToString());

            return gymEvents;
        }

        public async Task InsertGymEvent(GymEvent newGymEvent, string authToken)
        {
            // Crea un JObject con los datos del evento
            JObject newGymEventAsJObject = JObject.FromObject(newGymEvent);

            // Crea un JObject que contiene un JWT firmado y encriptado
            string encryptedJwt = ApiUtils.CreateSignedAndEncryptedJwt(newGymEventAsJObject, secretKey);

            // Envía el token de autorización a la API y espera su respuesta
            await _gymEventApiConnector.InsertGymEvent(encryptedJwt, authToken);
        }

        public async Task UpdateGymEvent(int id, GymEvent updatedGymEvent, string authToken)
        {
            // Crea un JObject con los datos del evento actualizado
            JObject updatedGymEventAsJObject = JObject.FromObject(updatedGymEvent);

            // Crea un JObject que contiene un JWT firmado y encriptado
            string encryptedJwt = ApiUtils.CreateSignedAndEncryptedJwt(updatedGymEventAsJObject, secretKey);

            // Envía el token de autorización a la API y espera su respuesta
            await _gymEventApiConnector.UpdateGymEvent(id, encryptedJwt, authToken);
        }

        public async Task DeleteGymEvent(int deletedGymEvent, string authToken)
        {
            // Envía el token de autorización a la API y espera su respuesta
            await _gymEventApiConnector.DeleteGymEvent(deletedGymEvent, authToken);
        }
    }
}

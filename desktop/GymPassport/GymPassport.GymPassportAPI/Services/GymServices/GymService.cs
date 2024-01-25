using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.ApiConnectors;
using GymPassport.GymPassportAPI.Helpers;
using Jose;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Text;

namespace GymPassport.GymPassportAPI.Services.GymServices
{
    public class GymService : IGymService
    {
        // SECRET KEY ALMACENADA TEMPORALMENTE
        string secretKey = "__PROBANDO__probando__";
        // SECRET KEY ALMACENADA TEMPORALMENTE
        private readonly IGymApiConnector _gymApiConnector;

        public GymService(IGymApiConnector gymApiConnector)
        {
            _gymApiConnector = gymApiConnector;
        }

        /// <summary>
        /// Obtiene los clientes del gimnasio del usuario logueado.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Los clientes del gimnasio del usuario logueado.</returns>
        public async Task<ObservableCollection<Client>> GetAllGymClients(string authToken)
        {
            // Envía el token de autorización a la API y espera su respuesta
            JObject apiResponse = await _gymApiConnector.GetAllGymClients(authToken);

            // Almacena el JWT encriptado recibido
            string encryptedResponse = apiResponse["jwe"].ToString();

            // Desencripta el JWT recibido
            string decryptedResponse = AesGcmUtils.DecryptWithAESGCM(encryptedResponse, secretKey);

            // Decodificar el JWT recibido
            JObject jsonToken = JObject.Parse(JWT.Decode(decryptedResponse, Encoding.UTF8.GetBytes(secretKey), JwsAlgorithm.HS256));

            // Mapear los claims del JWT a una colección observable de la clase Client
            ObservableCollection<Client> clients = JsonConvert.DeserializeObject<ObservableCollection<Client>>(jsonToken["results"].ToString());

            return clients;
        }

        public async Task UpdateGym(Gym updatedGym, string authToken)
        {
            // Crea un JObject con los datos del gimnasio actualizado
            JObject updatedGymAsJObject = JObject.FromObject(updatedGym);

            // Crea un JObject que contiene un JWT firmado y encriptado
            string encryptedJwt = ApiUtils.CreateSignedAndEncryptedJwt(updatedGymAsJObject, secretKey);

            // Envía el token de autorización a la API y espera su respuesta
            await _gymApiConnector.UpdateGym(encryptedJwt, authToken);
        }
    }
}

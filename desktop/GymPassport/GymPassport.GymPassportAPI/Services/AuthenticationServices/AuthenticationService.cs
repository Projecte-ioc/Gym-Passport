using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.ApiConnectors;
using GymPassport.GymPassportAPI.Helpers;
using GymPassport.WPF.Exceptions;
using Jose;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace GymPassport.GymPassportAPI.Services.AuthenticationServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AppSettings _appSettings;
        private readonly LoginApiConnector _loginApiConnector;

        public AuthenticationService(IOptions<AppSettings> appSettings, LoginApiConnector loginApiConnector)
        {
            _appSettings = appSettings.Value;
            _loginApiConnector = loginApiConnector;
        }

        public async Task<Account> Login(string loginUsername, string loginPassword)
        {
            // SECRET KEY ALMACENADA TEMPORALMENTE
            string secretKey = "__PROBANDO__probando__";
            // SECRET KEY ALMACENADA TEMPORALMENTE

            const int COUNT_LIMIT = 5;
            string url = $"http://10.2.190.11:4000/login";

            // Crea un JObject con los datos del login
            JObject loginDataObject = new JObject
            {
                { "pswd_app", loginPassword },
                { "user_name", loginUsername }
            };

            // Crea un string que contiene un JWT firmado y encriptado
            string encryptedJwt = ApiUtils.CreateSignedAndEncryptedJwt(loginDataObject, secretKey);

            for (int i = 0; i < COUNT_LIMIT; i++)
            {
                try
                {
                    // Envía el JObject con el JWT a la API y espera su respuesta
                    JObject apiResponse = await _loginApiConnector.Login(encryptedJwt);

                    // Almacena el JWT encriptado recibido
                    string encryptedResponse = apiResponse["jwe"].ToString();

                    // Desencripta el JWT recibido
                    string decryptedResponse = AesGcmUtils.DecryptWithAESGCM(encryptedResponse, secretKey);

                    // Decodificar el JWT recibido
                    string jsonToken = JWT.Decode(decryptedResponse, Encoding.UTF8.GetBytes(secretKey), JwsAlgorithm.HS256);

                    // Mapear los claims del JWT a una instancia de la clase Account
                    Account account = JsonConvert.DeserializeObject<Account>(jsonToken);

                    // Almaceno en la cuenta el token que se necesitará en futuras llamadas a la API
                    account.AuthToken = encryptedResponse;

                    // Devuelve la instancia de la clase Account
                    return account;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message + $"\nReintentando conexión (intento nº{i}");
                }
            }

            throw new UserNotFoundException(loginUsername);
        }
    }
}

using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.ApiConnectors;
using GymPassport.GymPassportAPI.Helpers;
using GymPassport.WPF.Exceptions;
using Jose;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace GymPassport.GymPassportAPI.Services.AuthenticationServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly LoginApiConnector _loginApiConnector;

        public AuthenticationService(LoginApiConnector loginApiConnector)
        {
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

                    //string content = "";

                    //using (HttpClient = new HttpClient())
                    //{
                    //    using var httpResponse = await HttpClient.PostAsJsonAsync(url, loginData);
                    //    switch (httpResponse.StatusCode)
                    //    {
                    //        case System.Net.HttpStatusCode.OK:
                    //            content = await httpResponse.Content.ReadAsStringAsync();
                    //            break;
                    //        default:
                    //            Console.WriteLine(httpResponse.ToString());
                    //            break;
                    //    }
                    //}

                    //// Envía los datos de login a la API y si son correctos recibe un token
                    //dynamic r = JObject.Parse(content);
                    //string jwtToken = r["token"];

                    //// Si el token no es null, extraemos los claims y los convertimos en un obejto Account
                    //if (jwtToken != null)
                    //{
                    //    var token = jwtToken;
                    //    var jwtSecurityToken = new JwtSecurityToken(token);
                    //    foreach (var item in jwtSecurityToken.Claims)
                    //    {
                    //        switch (item.Type)
                    //        {
                    //            case "user_name":
                    //                account.Username = item.Value;
                    //                break;
                    //            case "rol_user":
                    //                account.Role = item.Value;
                    //                break;
                    //            case "gym_name":
                    //                account.GymName = item.Value;
                    //                break;
                    //            case "name":
                    //                account.Name = item.Value;
                    //                break;
                    //            default:
                    //                break;
                    //        }
                    //    }

                    //    account.Token = token;

                    //    return account;
                    //}

                    //break;
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

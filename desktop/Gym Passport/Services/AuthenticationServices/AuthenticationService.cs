using Gym_Passport.Exceptions;
using Gym_Passport.Models;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gym_Passport.Services
{
    public class AuthenticationService : ServiceBase, IAuthenticationService
    {
        public async Task<Account> Login(string loginUsername, string loginPassword)
        {
            const int COUNT_LIMIT = 5;
            string url = $"{BaseURL}4000/login";

            var loginData = new
            {
                user_name = loginUsername,
                pswd_app = loginPassword
            };

            Account account = new Account();

            for (int i = 0; i < COUNT_LIMIT; i++)
            {
                try
                {
                    string content = "";

                    using (HttpClient = new HttpClient())
                    {
                        using var httpResponse = await HttpClient.PostAsJsonAsync(url, loginData);
                        switch (httpResponse.StatusCode)
                        {
                            case System.Net.HttpStatusCode.OK:
                                content = await httpResponse.Content.ReadAsStringAsync();
                                break;
                            default:
                                Console.WriteLine(httpResponse.ToString());
                                break;
                        }
                    }

                    // Envía los datos de login a la API y si son correctos recibe un token
                    dynamic r = JObject.Parse(content);
                    string jwtToken = r["token"];

                    // Si el token no es null, extraemos los claims y los convertimos en un obejto Account
                    if (jwtToken != null)
                    {
                        var token = jwtToken;
                        var jwtSecurityToken = new JwtSecurityToken(token);
                        foreach (var item in jwtSecurityToken.Claims)
                        {
                            switch (item.Type)
                            {
                                case "user_name":
                                    account.Username = item.Value;
                                    break;
                                case "rol_user":
                                    account.Role = item.Value;
                                    break;
                                case "gym_name":
                                    account.GymName = item.Value;
                                    break;
                                case "name":
                                    account.Name = item.Value;
                                    break;
                                default:
                                    break;
                            }
                        }

                        account.Token = token;

                        return account;
                    }

                    break;
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

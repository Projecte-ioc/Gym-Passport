using Gym_Passport_Navigation.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Gym_Passport_Navigation.Services
{
    public class AuthenticationService : ServiceBase, IAuthenticationService
    {
        public async Task<Account> Login(string loginUsername, string loginPassword)
        {
            Account account = new Account();

            string jwtToken = await GetToken(loginUsername, loginPassword);

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

            return null;
        }

        public async Task<string> GetToken(string loginUsername, string loginPassword)
        {
            var loginData = new
            {
                user_name = loginUsername,
                pswd_app = loginPassword
            };

            string url = $"{BaseURL}4000/login";

            try
            {
                dynamic r = JObject.Parse(await PostAsJSONAsync(url, loginData));

                Console.WriteLine(r.ToString());
                Console.WriteLine("");

                string jwtToken = r["token"];
                return jwtToken;
            }
            catch (JsonReaderException ex)
            {
                return null;
            }
        }
    }
}

using Gym_Passport.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System.Security;

namespace Gym_Passport.Services
{
    public class AuthenticationService : ServiceBase, IAuthenticationService
    {
        public async Task<UserAccountModel> Login(string loginUsername, SecureString loginPassword)
        {
            string username = "";
            string userRole = "";
            string gymName = "";
            string name = "";

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
                            username = item.Value;
                            break;
                        case "rol_user":
                            userRole = item.Value;
                            break;
                        case "gym_name":
                            gymName = item.Value;
                            break;
                        case "name":
                            name = item.Value;
                            break;
                        default:
                            break;
                    }
                }

                return new UserAccountModel(username, userRole, gymName, name, token);
            }

            return null;
        }

        public async Task<string> GetToken(string loginUsername, SecureString loginPassword)
        {
            var loginData = new
            {
                user_name = loginUsername,
                pswd_app = new System.Net.NetworkCredential(string.Empty, loginPassword).Password
            };

            string url = $"{BaseURL}/login";

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

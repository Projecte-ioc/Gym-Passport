using GymPassport.Domain.Models;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GymPassport.GymPassportAPI.Services.ProfileServices
{
    public class ProfileService : ServiceBase, IProfileService
    {
        #region /profile_info
        public async Task<UserProfile> GetAllProfileInfo(string accessToken)
        {
            string httpResponseContent = await GetHttpResponseContent(accessToken);
            string profileInfoToken = GetProfileInfoToken(httpResponseContent);
            return GetUserProfile(profileInfoToken);
        }

        public UserProfile GetUserProfile(string jwtToken)
        {
            UserProfile userProfileModel = new UserProfile();

            if (jwtToken != null)
            {
                var token = jwtToken;
                var jwtSecurityToken = new JwtSecurityToken(token);
                foreach (var item in jwtSecurityToken.Claims)
                {
                    switch (item.Type)
                    {
                        case "user_name":
                            userProfileModel.Username = item.Value;
                            break;
                        case "rol_user":
                            userProfileModel.UserRole = item.Value;
                            break;
                        case "gym_name":
                            userProfileModel.GymName = item.Value;
                            break;
                        case "address":
                            userProfileModel.GymAddress = item.Value;
                            break;
                        case "phone_number":
                            userProfileModel.GymPhoneNumber = item.Value;
                            break;
                        case "schedule":
                            userProfileModel.GymSchedules.Add(item.Value);
                            break;
                        default:
                            break;
                    }
                }

                return userProfileModel;
            }

            return null;
        }

        public string GetProfileInfoToken(string httpResponseContent)
        {
            dynamic r = JObject.Parse(httpResponseContent);
            string jwtToken = r["ad-token"];
            return jwtToken;
        }

        public async Task<string> GetHttpResponseContent(string accessToken)
        {
            string URL = $"{BaseURL}3000/profile_info";

            using (HttpClient = new HttpClient())
            {
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken);

                using HttpResponseMessage httpResponseMessage = await HttpClient.GetAsync(URL);
                return await httpResponseMessage.Content.ReadAsStringAsync();
            }
        }
        #endregion

        public async Task<string> GetHttpResponseContent(object requestData)
        {
            string URL = $"{BaseURL}4000/login";

            using (HttpClient = new HttpClient())
            {
                using var httpResponseMessage = await HttpClient.PostAsJsonAsync(URL, requestData);
                return await httpResponseMessage.Content.ReadAsStringAsync();
            }
        }
    }
}

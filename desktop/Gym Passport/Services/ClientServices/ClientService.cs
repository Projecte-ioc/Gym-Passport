﻿using Gym_Passport.Models;
using Gym_Passport.Services;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GymPassportPruebasAPI.Services.ClientServices
{
    public class ClientService : ServiceBase, IClientService
    {
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

        public async Task<string> InsertClient(string accessToken, object client)
        {
            string URL = $"{BaseURL}3000/insert_client";

            dynamic json = JObject.FromObject(client);
            var httpContent = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

            using (HttpClient = new HttpClient())
            {
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken);
                HttpResponseMessage httpResponseMessage = await HttpClient.PostAsync(URL, httpContent);
                MessageBox.Show(httpResponseMessage.ToString());
                return await httpResponseMessage.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> UpdateClient(string accessToken, object client)
        {
            string URL = $"{BaseURL}3000/update_data_client";

            dynamic json = JObject.FromObject(client);
            var httpContent = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

            using (HttpClient = new HttpClient())
            {
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken);
                HttpResponseMessage httpResponseMessage = await HttpClient.PutAsync(URL, httpContent);
                return await httpResponseMessage.Content.ReadAsStringAsync();
            }
        }

        public async Task<bool> DeleteClient(string accessToken, string username)
        {
            string URL = $"{BaseURL}3000/delete_client?user_name={username}";

            Console.WriteLine(URL);

            using (HttpClient = new HttpClient())
            {
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken);

                HttpResponseMessage httpResponseMessage = await HttpClient.DeleteAsync(URL);

                Console.WriteLine(httpResponseMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync();

                    dynamic r = JObject.Parse(httpResponseContent);
                    string json = r["message"];
                    Console.WriteLine(r.ToString());
                    return true;
                }
            }
            return false;
        }
    }
}
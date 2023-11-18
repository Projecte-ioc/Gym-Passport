using Gym_Passport_Navigation.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Gym_Passport_Navigation.Services.GymServices
{
    public class GymService : ServiceBase, IGymService
    {
        /// <summary>
        /// Obtiene los clientes del gimnasio del usuario logueado.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Los clientes del gimnasio del usuario logueado.</returns>
        public async Task<ObservableCollection<Client>> GetAllGymClients(string token)
        {
            string URL = $"{BaseURL}2000/consultar_clientes_gym";

            using (HttpClient = new HttpClient())
            {
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);

                HttpResponseMessage httpResponseMessage = await HttpClient.GetAsync(URL);

                string json = await httpResponseMessage.Content.ReadAsStringAsync();
                ObservableCollection<Client> clients = JsonConvert.DeserializeObject<ObservableCollection<Client>>(json);

                return clients;
            }
        }

        public async Task<string> UpdateGym(string accessToken, Gym gym)
        {
            string URL = $"{BaseURL}2000/update_gym";

            var updatedGym = new
            {
                address = gym.Address,
                phone_number = gym.PhoneNumber,
                schedule = gym.Schedule
            };

            dynamic json = JObject.FromObject(updatedGym);
            var httpContent = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

            using (HttpClient = new HttpClient())
            {
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken);
                HttpResponseMessage httpResponseMessage = await HttpClient.PatchAsync(URL, httpContent);
                Console.WriteLine(httpResponseMessage);
                return await httpResponseMessage.Content.ReadAsStringAsync();
            }
        }
    }
}

using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace GymPassport.Domain.Models
{
    public class UserProfile
    {
        [JsonProperty("user_name")]
        public string Username { get; set; }

        [JsonProperty("rol_user")]
        public string UserRole { get; set; }

        [JsonProperty("gym_name")]
        public string GymName { get; set; }

        [JsonProperty("address")]
        public string GymAddress { get; set; }

        [JsonProperty("phone_number")]
        public string GymPhoneNumber { get; set; }

        [JsonProperty("schedule")]
        public ObservableCollection<string> GymSchedules { get; set; } = new ObservableCollection<string>();

        public override string ToString()
        {
            string str = $"Username: {Username}\n" +
                $"UserRole: {UserRole}\n" +
                $"GymName: {GymName}\n" +
                $"GymAddress: {GymAddress}\n" +
                $"GymPhoneNumber: {GymPhoneNumber}\n" +
                $"GymSchedules:\n";

            foreach (var item in GymSchedules)
            {
                str += item;
                str += "\n";
            }

            return str;
        }
    }
}

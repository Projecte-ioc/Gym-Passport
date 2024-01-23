using Newtonsoft.Json;

namespace GymPassport.Domain.Models
{
    public class Account
    {
        [JsonProperty("user_name")]
        public string Username { get; set; }

        [JsonProperty("rol_user")]
        public string Role { get; set; }

        [JsonProperty("gym_name")]
        public string GymName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonIgnore] // Exclude AuthToken from JSON serialization
        public string AuthToken { get; set; }

        public Account(string Username, string Role, string GymName, string Name, string AuthToken)
        {
            this.Username = Username;
            this.Role = Role;
            this.GymName = GymName;
            this.Name = Name;
            this.AuthToken = AuthToken;
        }

        public override string ToString()
        {
            return
                $"Username:\t{Username}\n" +
                $"User role:\t{Role}\n" +
                $"Gym name:\t{GymName}\n" +
                $"Name:\t\t{Name}\n" +
                $"AuthToken:\t\t{AuthToken}";
        }
    }
}

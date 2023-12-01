using Newtonsoft.Json;

namespace GymPassport.Domain.Models
{
    public class Client
    {
        public Client(string name, string role, string username, string password)
        {
            Name = name;
            Role = role;
            Username = username;
            Password = password;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("role_user")]
        public string Role { get; set; }

        [JsonProperty("user_name")]
        public string Username { get; set; }

        [JsonProperty("pswd_app")]
        public string Password { get; set; }

        public override string ToString()
        {
            return
                $"Name:\t  {Name}\n" +
                $"Username: {Username}\n" +
                $"Role:\t  {Role}\n";
        }
    }
}

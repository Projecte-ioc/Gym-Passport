using Newtonsoft.Json;

namespace Gym_Passport.Models
{
    public class Client
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("user_name")]
        public string Username { get; set; }
    }
}
using Newtonsoft.Json;

namespace GymPassport.Domain.Models
{
    public class LoginData
    {
        [JsonProperty("user_name")]
        public string Username { get; set; }

        [JsonProperty("pswd_app")]
        public string Password { get; set; }
    }
}

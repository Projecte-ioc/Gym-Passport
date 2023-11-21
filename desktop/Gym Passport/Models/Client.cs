using Newtonsoft.Json;
using System;

namespace Gym_Passport.Models
{
    public class Client : ICloneable
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("role")]
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

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
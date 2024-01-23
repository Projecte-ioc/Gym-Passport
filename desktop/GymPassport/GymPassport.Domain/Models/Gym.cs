using Newtonsoft.Json;

namespace GymPassport.Domain.Models
{
    public class Gym
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("schedule")]
        public string[] Schedule { get; set; }
    }
}

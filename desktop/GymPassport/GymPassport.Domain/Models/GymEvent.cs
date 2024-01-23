using Newtonsoft.Json;

namespace GymPassport.Domain.Models
{
    public class GymEvent
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("whereisit")]
        public string Location { get; set; }

        [JsonProperty("qty_max_attendes")]
        public int NumParticipants { get; set; }

        [JsonProperty("qty_got_it")]
        public int NumAttendances { get; set; }

        [JsonProperty("user_id")]
        public int ClientId { get; set; }

        [JsonProperty("gym_id")]
        public int GymId { get; set; }

        [JsonProperty("done")]
        public bool Done { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("hour")]
        public int Hour { get; set; }

        [JsonProperty("minute")]
        public int Minute { get; set; }

        [JsonIgnore] // Ignora la propiedad en la serialización JSON
        public DateTime DateTime
        {
            get
            {
                return new DateTime(
                    int.Parse(Date.Split('-')[0]),
                    int.Parse(Date.Split('-')[1]),
                    int.Parse(Date.Split('-')[2]),
                    Hour,
                    Minute,
                    0);
            }
        }

        public override string ToString()
        {
            string str = $"Id: {Id}\n" +
                         $"Name: {Name}\n" +
                         $"Location: {Location}\n" +
                         $"NumParticipants: {NumParticipants}\n" +
                         $"NumAttendances: {NumAttendances}\n" +
                         $"ClientId: {ClientId}\n" +
                         $"GymId: {GymId}\n" +
                         $"Done: {Done}\n" +
                         $"Date: {Date}\n" +
                         $"Hour: {Hour}\n" +
                         $"Minute: {Minute}\n" +
                         $"DateTime: {DateTime}\n";

            return str;
        }
    }
}

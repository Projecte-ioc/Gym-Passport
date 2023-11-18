using System.Collections.ObjectModel;

namespace Gym_Passport.Models
{
    public class UserProfile
    {
        public string Username { get; set; }
        public string UserRole { get; set; }
        public string GymName { get; set; }
        public string GymAddress { get; set; }
        public string GymPhoneNumber { get; set; }
        public ObservableCollection<string> GymSchedules { get; set; } = new ObservableCollection<string>();

        public override string ToString()
        {
            string str = $"Username: {Username}\n" +
                $"UserRole: {UserRole}\n" +
                $"GymName: {GymName}\n" +
                $"GymAddress: {GymAddress}\n" +
                $"GymPhoneNumber: {GymPhoneNumber}\n" +
                $"GymScheduleString:\n";

            foreach (var item in GymSchedules)
            {
                str += item;
                str += "\n";
            }

            return str;
        }
    }
}

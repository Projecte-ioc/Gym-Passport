namespace Gym_Passport.ViewModels
{
    public class ClientViewModel : ViewModelBase
    {
        public ClientViewModel(string name, string username, string role)
        {
            Name = name;
            Username = username;
            Role = role;
        }

        public string Name { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}

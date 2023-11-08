namespace Gym_Passport.Models
{
    public class UserAccountModel
    {
        public UserAccountModel(string username, string role, string gymName, string name, string token)
        {
            Username = username;
            Role = role;
            GymName = gymName;
            Name = name;
            Token = token;
        }

        public string Username { get; set; }
        public string Role { get; set; }
        public string GymName { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
    }
}

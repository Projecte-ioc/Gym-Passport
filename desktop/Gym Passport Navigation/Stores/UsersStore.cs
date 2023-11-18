using System;

namespace Gym_Passport_Navigation.Stores
{
    public class UsersStore
    {
        public event Action<string, string, string> UserAdded;

        public void AddUser(string name, string username, string role)
        {
            UserAdded?.Invoke(name, username, role);
        }
    }
}

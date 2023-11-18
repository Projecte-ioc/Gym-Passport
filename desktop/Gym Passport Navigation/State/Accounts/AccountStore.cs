using Gym_Passport_Navigation.Models;
using System;

namespace Gym_Passport_Navigation.State.Accounts
{
    public class AccountStore : IAccountStore
    {
        private Account _currentAccount;
        public Account CurrentAccount
        {
            get
            {
                return _currentAccount;
            }
            set
            {
                _currentAccount = value;
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;
    }
}

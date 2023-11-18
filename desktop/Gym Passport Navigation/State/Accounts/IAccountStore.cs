using Gym_Passport_Navigation.Models;
using System;

namespace Gym_Passport_Navigation.State.Accounts
{
    public interface IAccountStore
    {
        Account CurrentAccount { get; set; }

        event Action StateChanged;
    }
}
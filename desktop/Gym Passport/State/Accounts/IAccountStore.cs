using Gym_Passport.Models;
using System;

namespace Gym_Passport.State.Accounts
{
    public interface IAccountStore
    {
        Account CurrentAccount { get; set; }

        event Action StateChanged;
    }
}
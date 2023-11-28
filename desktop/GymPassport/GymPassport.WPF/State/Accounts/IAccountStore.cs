using GymPassport.WPF.Models;

namespace GymPassport.WPF.State.Accounts
{
    public interface IAccountStore
    {
        Account CurrentAccount { get; set; }

        event Action StateChanged;
    }
}

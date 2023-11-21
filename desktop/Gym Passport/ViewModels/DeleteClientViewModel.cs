using Gym_Passport.Commands;
using Gym_Passport.Services;
using Gym_Passport.State.Accounts;
using Gym_Passport.ViewModels;
using Gym_Passport_Navigation.Commands;
using GymPassportPruebasAPI.Services.ClientServices;
using System.Windows.Input;

namespace Gym_Passport_Navigation.ViewModels
{
    public class DeleteClientViewModel : ViewModelBase
    {
        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        public DeleteClientViewModel(INavigationService closeNavigationService, IAccountStore accountStore, IClientService clientService)
        {
            SubmitCommand = new DeleteClientCommand(this, accountStore, clientService, closeNavigationService);
            CancelCommand = new NavigateCommand(closeNavigationService);
        }
    }
}

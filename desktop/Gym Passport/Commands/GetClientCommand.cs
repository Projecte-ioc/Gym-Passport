using Gym_Passport.Commands;
using Gym_Passport.Models;
using Gym_Passport.Services.GymServices;
using Gym_Passport.State.Accounts;
using Gym_Passport.ViewModels;

namespace Gym_Passport_Navigation.Commands
{
    public class GetClientCommand : CommandBase
    {
        private readonly ClientsViewModel _clientsViewModel;

        public GetClientCommand(ClientsViewModel clientsViewModel)
        {
            _clientsViewModel = clientsViewModel;
        }

        public override void Execute(object? parameter)
        {
            _clientsViewModel.CurrentClient = (Client)parameter;
        }
    }
}

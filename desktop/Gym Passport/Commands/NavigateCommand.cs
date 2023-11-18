using Gym_Passport.Services;
using Gym_Passport.Stores;
using Gym_Passport.ViewModels;
using System;

namespace Gym_Passport.Commands
{
    public class NavigateCommand : CommandBase
    {
        private readonly INavigationService _navigationService;

        public NavigateCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            _navigationService.Navigate();
        }
    }
}

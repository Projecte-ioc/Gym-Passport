using Gym_Passport_Navigation.Services;
using Gym_Passport_Navigation.Stores;
using Gym_Passport_Navigation.ViewModels;
using System;

namespace Gym_Passport_Navigation.Commands
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

using Gym_Passport_Navigation.Commands;
using Gym_Passport_Navigation.Services;
using Gym_Passport_Navigation.Stores;
using System.Windows.Input;

namespace Gym_Passport_Navigation.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public string WelcomeMessage => "Welcome to my application.";

        public ICommand NavigateLoginCommand { get; }

        public HomeViewModel(INavigationService loginNavigationService)
        {
            NavigateLoginCommand = new NavigateCommand(loginNavigationService);
        }
    }
}

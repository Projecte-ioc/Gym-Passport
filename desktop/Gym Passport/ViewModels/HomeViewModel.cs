using Gym_Passport.Commands;
using Gym_Passport.Services;
using Gym_Passport.Stores;
using System.Windows.Input;

namespace Gym_Passport.ViewModels
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

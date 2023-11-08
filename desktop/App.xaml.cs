using Gym_Passport.Models;
using Gym_Passport.View;
using Gym_Passport.ViewModels;
using Gym_Passport.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace Gym_Passport
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            var loginView = new LoginView();
            loginView.Show();
            loginView.IsVisibleChanged += (s, ev) =>
            {
                if (loginView.IsVisible == false && loginView.IsLoaded)
                {
                    LoginViewModel loginViewModel = (LoginViewModel)loginView.DataContext;

                    var mainView = new MainView()
                    {
                        DataContext = new MainViewModel(loginViewModel.CurrentAccount)
                    };

                    mainView.Show();
                    loginView.Close();
                }
            };
        }
    }
}

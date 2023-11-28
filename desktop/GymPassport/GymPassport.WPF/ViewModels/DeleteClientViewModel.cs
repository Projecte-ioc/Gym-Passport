using GymPassport.WPF.Commands;
using GymPassport.WPF.Services;
using GymPassport.WPF.Services.ClientServices;
using GymPassport.WPF.State.Accounts;
using GymPassport.WPF.Stores;
using System.Windows.Input;

namespace GymPassport.WPF.ViewModels
{
    public class DeleteClientViewModel : ViewModelBase
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _username;
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _role;
        public string Role
        {
            get
            {
                return _role;
            }
            set
            {
                _role = value;
                OnPropertyChanged(nameof(Role));
            }
        }

        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        public DeleteClientViewModel(INavigationService closeNavigationService, ClientsStore clientsStore, IAccountStore accountStore, IClientService clientService)
        {
            SubmitCommand = new DeleteClientCommand(this, clientsStore, accountStore, clientService, closeNavigationService);
            CancelCommand = new NavigateCommand(closeNavigationService);
        }
    }
}

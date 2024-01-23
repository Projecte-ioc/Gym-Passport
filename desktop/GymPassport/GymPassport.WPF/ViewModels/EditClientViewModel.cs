using GymPassport.Domain.Models;
using GymPassport.WPF.Commands;
using GymPassport.WPF.State.Clients;
using GymPassport.WPF.State.Navigators;
using System.Windows.Input;

namespace GymPassport.WPF.ViewModels
{
    public class EditClientViewModel : ViewModelBase
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
                OnPropertyChanged(nameof(CanSubmit));
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

        private bool _isSubmitting;
        public bool IsSubmitting
        {
            get
            {
                return _isSubmitting;
            }
            set
            {
                _isSubmitting = value;
                OnPropertyChanged(nameof(IsSubmitting));
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        public bool CanSubmit => !string.IsNullOrEmpty(Username);

        public String ClientUsername { get; }

        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        public EditClientViewModel(Client client, ClientsStore _clientsStore, ModalNavigationStore modalNavigationStore)
        {
            ClientUsername = client.Username;

            SubmitCommand = new EditClientCommand(this, _clientsStore, modalNavigationStore);
            CancelCommand = new CloseModalCommand(modalNavigationStore);

            Name = client.Name;
            Role = client.Role;
            Username = client.Username;
        }
    }
}

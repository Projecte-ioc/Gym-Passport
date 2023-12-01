using GymPassport.Domain.Models;
using GymPassport.WPF.Commands;
using GymPassport.WPF.State;
using GymPassport.WPF.State.Navigators;
using System.Windows.Input;

namespace GymPassport.WPF.ViewModels
{
    public class ClientsListingItemViewModel : ViewModelBase
    {
        public Client Client { get; private set; }

        public string Username => Client.Username;

        private bool _isDeleting;
        public bool IsDeleting
        {
            get
            {
                return _isDeleting;
            }
            set
            {
                _isDeleting = value;
                OnPropertyChanged(nameof(IsDeleting));
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

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public ClientsListingItemViewModel(Client client, ClientsStore clientsStore, ModalNavigationStore modalNavigationStore)
        {
            Client = client;

            EditCommand = new OpenEditClientCommand(this, clientsStore, modalNavigationStore);
            DeleteCommand = new DeleteClientCommand(this, clientsStore);
        }

        public void Update(Client client)
        {
            Client = client;
            OnPropertyChanged(nameof(Username));
        }
    }
}

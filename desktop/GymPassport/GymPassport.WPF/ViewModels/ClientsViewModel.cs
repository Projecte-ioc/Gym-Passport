using GymPassport.WPF.Commands;
using GymPassport.WPF.State;
using GymPassport.WPF.State.Navigators;
using System.Windows.Input;

namespace GymPassport.WPF.ViewModels
{
    public class ClientsViewModel : ViewModelBase
    {
        public ClientsListingViewModel ClientsListingViewModel { get; }
        public ClientsDetailsViewModel ClientsDetailsViewModel { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
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

        public ICommand LoadClientsCommand { get; }
        public ICommand AddClientsCommand { get; }

        public ClientsViewModel(ClientsStore clientsStore, SelectedClientStore _selectedClientStore, ModalNavigationStore modalNavigationStore)
        {
            ClientsListingViewModel = new ClientsListingViewModel(clientsStore, _selectedClientStore, modalNavigationStore);
            ClientsDetailsViewModel = new ClientsDetailsViewModel(_selectedClientStore);

            LoadClientsCommand = new LoadClientsCommand(this, clientsStore);
            AddClientsCommand = new OpenAddClientCommand(clientsStore, modalNavigationStore);
        }

        public static ClientsViewModel LoadViewModel(ClientsStore clientsStore, SelectedClientStore selectedClientStore, ModalNavigationStore modalNavigationStore)
        {
            ClientsViewModel viewModel = new ClientsViewModel(clientsStore, selectedClientStore, modalNavigationStore);
            viewModel.LoadClientsCommand.Execute(null);
            return viewModel;
        }
    }
}

using GymPassport.WPF.Commands;
using GymPassport.WPF.Services;
using GymPassport.WPF.Services.ClientServices;
using GymPassport.WPF.Services.GymServices;
using GymPassport.WPF.State.Accounts;
using GymPassport.WPF.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Navigation;

namespace GymPassport.WPF.ViewModels
{
    public class ClientsViewModel : ViewModelBase
    {
        private readonly ClientsStore _clientsStore;

        private string _btnToggleModificationText;
        public string btnToggleModificationText
        {
            get
            {
                return _btnToggleModificationText;
            }
            set
            {
                _btnToggleModificationText = value;
                OnPropertyChanged(nameof(btnToggleModificationText));
            }
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        private ObservableCollection<ClientViewModel> _clients;
        public ObservableCollection<ClientViewModel> Clients
        {
            get
            {
                return _clients;
            }
            set
            {
                _clients = value;
                OnPropertyChanged(nameof(Clients));
            }
        }

        //private readonly ObservableCollection<ClientViewModel> _clients;
        //public ObservableCollection<ClientViewModel> Clients => _clients;

        //private Client _currentClient;
        //public Client CurrentClient
        //{
        //    get
        //    {
        //        return _currentClient;
        //    }
        //    set
        //    {
        //        _currentClient = value;
        //        OnPropertyChanged(nameof(CurrentClient));
        //        if (this._currentClient != null)
        //        {
        //            this.Roles_SelectedValue = _currentClient.Role;
        //        }
        //    }
        //}

        private ClientViewModel _currentClient;
        public ClientViewModel CurrentClient
        {
            get
            {
                return _currentClient;
            }
            set
            {
                _currentClient = value;
                OnPropertyChanged(nameof(CurrentClient));
                if (this._currentClient != null)
                {
                    this.Roles_SelectedValue = _currentClient.Role;
                }
            }
        }

        private ObservableCollection<string> _roles = new ObservableCollection<string>();

        public ObservableCollection<string> Roles
        {
            get { return _roles; }
        }

        private string _roles_SelectedValue;
        public string Roles_SelectedValue
        {
            get
            {
                return _roles_SelectedValue;
            }
            set
            {
                _roles_SelectedValue = value;
                OnPropertyChanged(nameof(Roles_SelectedValue));
            }
        }

        public ICommand GetAllClientsCommand { get; }
        public ICommand GetClientCommand { get; }
        public ICommand EnableClientModificationCommand { get; }
        public ICommand UpdateClientCommand { get; }
        public ICommand DeleteClientCommand { get; }
        public ICommand ShowAddClientViewCommand { get; }
        public ICommand ShowDeleteClientViewCommand { get; }

        public ClientsViewModel(
            ClientsStore clientsStore,
            IAccountStore accountStore,
            IGymService gymService,
        IClientService clientService,
            INavigationService addClientNavigationService,
            INavigationService deleteClientNavigationService)
        {
            _clientsStore = clientsStore;
            _clients = new ObservableCollection<ClientViewModel>();

            _clientsStore.ClientAdded += OnClientAdded;
            _clientsStore.ClientRemoved += OnClientRemoved;

            btnToggleModificationText = "Desbloquejar modificació";
            IsEnabled = false;

            EnableClientModificationCommand = new EnableClientModificationCommand(this);
            GetAllClientsCommand = new GetAllClientsCommand(this, gymService, accountStore);
            GetClientCommand = new GetClientCommand(this);
            //UpdateClientCommand = new UpdateClientCommand(this);
            ShowAddClientViewCommand = new NavigateCommand(addClientNavigationService);
            ShowDeleteClientViewCommand = new NavigateCommand(deleteClientNavigationService);
        }

        private void OnClientAdded(ClientViewModel client)
        {
            _clients.Add(client);
        }

        private void OnClientRemoved(ClientViewModel client)
        {
            _clients.Remove(client);
        }
    }
}

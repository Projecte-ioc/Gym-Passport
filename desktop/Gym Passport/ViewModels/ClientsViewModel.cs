using Gym_Passport.Commands;
using Gym_Passport.Models;
using Gym_Passport.Services.GymServices;
using Gym_Passport.State.Accounts;
using Gym_Passport_Navigation.Commands;
using GymPassportPruebasAPI.Services.ClientServices;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gym_Passport.ViewModels
{
    public class ClientsViewModel : ViewModelBase
    {
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

        private ObservableCollection<Client> _clients;
        public ObservableCollection<Client> Clients
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

        private Client _currentClient;
        public Client CurrentClient
        {
            get
            {
                return _currentClient;
            }
            set
            {
                _currentClient = value;
                OnPropertyChanged(nameof(CurrentClient));
                if(this._currentClient != null)
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
        public ICommand AddClientCommand { get; }
        public ICommand UpdateClientCommand { get; }
        public ICommand DeleteClientCommand { get; }

        public ClientsViewModel(IAccountStore accountStore, IGymService gymService, IClientService clientService)
        {
            btnToggleModificationText = "Desbloquejar modificació";
            IsEnabled = false;

            EnableClientModificationCommand = new EnableClientModificationCommand(this);
            GetAllClientsCommand = new GetAllClientsCommand(this, gymService, accountStore);
            GetClientCommand = new GetClientCommand(this);
            AddClientCommand = new AddClientCommand(this, accountStore, clientService);
            //UpdateClientCommand = new UpdateClientCommand(this);
            //DeleteClientCommand = new DeleteClientCommand(this);
        }
    }
}
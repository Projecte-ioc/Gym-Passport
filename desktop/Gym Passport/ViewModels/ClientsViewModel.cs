using Gym_Passport.Commands;
using Gym_Passport.Models;
using Gym_Passport.Services.GymServices;
using Gym_Passport.State.Accounts;
using Gym_Passport_Navigation.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Input;
using System.Linq;
using Gym_Passport_Navigation.Utils;

namespace Gym_Passport.ViewModels
{
    public class ClientsViewModel : ViewModelBase
    {
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
                this.OnPropertyChanged(nameof(CurrentClient));
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

        public ClientsViewModel(IAccountStore accountStore, IGymService gymService)
        {
            GetAllClientsCommand = new GetAllClientsCommand(this, gymService, accountStore);
            GetClientCommand = new GetClientCommand(this);
        }
    }
}
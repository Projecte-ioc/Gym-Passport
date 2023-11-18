using Gym_Passport.Commands;
using Gym_Passport.Models;
using Gym_Passport.Services;
using Gym_Passport.Services.GymServices;
using Gym_Passport.Services.ProfileServices;
using Gym_Passport.State.Accounts;
using Gym_Passport.Stores;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gym_Passport.ViewModels
{
    public class ClientsViewModel : ViewModelBase
    {
        private readonly IAccountStore _accountStore;
        private readonly IGymService _gymService;

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

        public ICommand GetClientsCommand { get; }

        public ClientsViewModel(IAccountStore accountStore, IGymService gymService)
        {
            _accountStore = accountStore;
            _gymService = gymService;
            GetClientsCommand = new GetClientsCommand(this, gymService, accountStore);
            GetClientsCommand.Execute(null);
        }
    }
}
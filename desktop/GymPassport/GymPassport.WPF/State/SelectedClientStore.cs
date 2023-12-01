using GymPassport.Domain.Models;

namespace GymPassport.WPF.State
{
    public class SelectedClientStore
    {
        private readonly ClientsStore _clientsStore;

        private Client _selectedClient;
        public Client SelectedClient
        {
            get
            {
                return _selectedClient;
            }
            set
            {
                _selectedClient = value;
                SelectedClientChanged?.Invoke();
            }
        }

        public event Action SelectedClientChanged;

        public SelectedClientStore(ClientsStore clientsStore)
        {
            _clientsStore = clientsStore;
            _clientsStore.ClientUpdated += ClientsStore_ClientUpdated;
            _clientsStore.ClientAdded += ClientsStore_ClientAdded;
        }

        private void ClientsStore_ClientAdded(Client client)
        {
            SelectedClient = client;
        }

        private void ClientsStore_ClientUpdated(Client client)
        {
            if (client.Username == SelectedClient?.Username)
            {
                SelectedClient = client;
            }
        }
    }
}

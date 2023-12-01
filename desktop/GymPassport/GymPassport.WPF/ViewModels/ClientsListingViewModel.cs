using GymPassport.Domain.Models;
using GymPassport.WPF.State;
using GymPassport.WPF.State.Navigators;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace GymPassport.WPF.ViewModels
{
    public class ClientsListingViewModel : ViewModelBase
    {
        private readonly ClientsStore _clientsStore;
        private readonly SelectedClientStore _selectedClientStore;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly ObservableCollection<ClientsListingItemViewModel> _clientsListingItemViewModels;

        public IEnumerable<ClientsListingItemViewModel> ClientsListingItemViewModels => _clientsListingItemViewModels;

        public ClientsListingItemViewModel SelectedClientListingItemViewModel
        {
            get
            {
                return _clientsListingItemViewModels
                    .FirstOrDefault(y => y.Client?.Username == _selectedClientStore.SelectedClient?.Username);
            }
            set
            {
                _selectedClientStore.SelectedClient = value?.Client;
            }
        }

        public ClientsListingViewModel(ClientsStore clientsStore, SelectedClientStore selectedClientStore, ModalNavigationStore modalNavigationStore)
        {
            _clientsStore = clientsStore;
            _selectedClientStore = selectedClientStore;
            _modalNavigationStore = modalNavigationStore;
            _clientsListingItemViewModels = new ObservableCollection<ClientsListingItemViewModel>();

            _selectedClientStore.SelectedClientChanged += SelectedClientStore_SelectedClientChanged;

            _clientsStore.ClientsLoaded += ClientsStore_ClientsLoaded;
            _clientsStore.ClientAdded += ClientsStore_ClientAdded;
            _clientsStore.ClientUpdated += ClientsStore_ClientUpdated;
            _clientsStore.ClientDeleted += ClientsStore_ClientDeleted;

            _clientsListingItemViewModels.CollectionChanged += ClientsListingItemViewModels_CollectionChanged;
        }

        protected override void Dispose()
        {
            _clientsListingItemViewModels.CollectionChanged -= ClientsListingItemViewModels_CollectionChanged;
            _selectedClientStore.SelectedClientChanged -= SelectedClientStore_SelectedClientChanged;
            _clientsStore.ClientsLoaded -= ClientsStore_ClientsLoaded;
            _clientsStore.ClientAdded -= ClientsStore_ClientAdded;
            _clientsStore.ClientUpdated -= ClientsStore_ClientUpdated;
            _clientsStore.ClientDeleted -= ClientsStore_ClientDeleted;

            base.Dispose();
        }

        private void SelectedClientStore_SelectedClientChanged()
        {
            OnPropertyChanged(nameof(SelectedClientListingItemViewModel));
        }

        private void ClientsStore_ClientsLoaded()
        {
            _clientsListingItemViewModels.Clear();

            foreach (Client client in _clientsStore.Clients)
            {
                AddClient(client);
            }
        }

        private void ClientsStore_ClientAdded(Client client)
        {
            AddClient(client);
        }

        private void ClientsStore_ClientUpdated(Client client)
        {
            ClientsListingItemViewModel clientViewModel =
                _clientsListingItemViewModels.FirstOrDefault(y => y.Client.Username == client.Username);

            if (clientViewModel != null)
            {
                clientViewModel.Update(client);
            }
        }

        private void ClientsStore_ClientDeleted(String username)
        {
            ClientsListingItemViewModel? itemViewModel = _clientsListingItemViewModels.FirstOrDefault(y => y.Client?.Username == username);

            if (itemViewModel != null)
            {
                _clientsListingItemViewModels.Remove(itemViewModel);
            }
        }

        private void ClientsListingItemViewModels_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(SelectedClientListingItemViewModel));
        }

        private void AddClient(Client client)
        {
            ClientsListingItemViewModel itemViewModel =
                new ClientsListingItemViewModel(client, _clientsStore, _modalNavigationStore);
            _clientsListingItemViewModels.Add(itemViewModel);
        }
    }
}

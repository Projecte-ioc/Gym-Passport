using GymPassport.Domain.Models;
using GymPassport.WPF.State;
using GymPassport.WPF.State.Navigators;
using GymPassport.WPF.ViewModels;

namespace GymPassport.WPF.Commands
{
    public class OpenEditClientCommand : CommandBase
    {
        private readonly ClientsListingItemViewModel _clientsListingItemViewModel;
        private readonly ClientsStore _clientsStore;
        private readonly ModalNavigationStore _modalNavigationStore;

        public OpenEditClientCommand(ClientsListingItemViewModel clientsListingItemViewModel,
            ClientsStore clientsStore,
            ModalNavigationStore modalNavigationStore)
        {
            _clientsListingItemViewModel = clientsListingItemViewModel;
            _clientsStore = clientsStore;
            _modalNavigationStore = modalNavigationStore;
        }

        public override void Execute(object parameter)
        {
            Client client = _clientsListingItemViewModel.Client;

            EditClientViewModel editClientViewModel =
                new EditClientViewModel(client, _clientsStore, _modalNavigationStore);
            _modalNavigationStore.CurrentViewModel = editClientViewModel;
        }
    }
}

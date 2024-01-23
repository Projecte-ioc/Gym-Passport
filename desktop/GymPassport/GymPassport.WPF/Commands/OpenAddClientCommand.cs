using GymPassport.WPF.State.Clients;
using GymPassport.WPF.State.Navigators;
using GymPassport.WPF.ViewModels;

namespace GymPassport.WPF.Commands
{
    public class OpenAddClientCommand : CommandBase
    {
        private readonly ClientsStore _clientsStore;
        private readonly ModalNavigationStore _modalNavigationStore;

        public OpenAddClientCommand(ClientsStore clientsStore, ModalNavigationStore modalNavigationStore)
        {
            _clientsStore = clientsStore;
            _modalNavigationStore = modalNavigationStore;
        }

        public override void Execute(object parameter)
        {
            AddClientViewModel addClientViewModel = new AddClientViewModel(_clientsStore, _modalNavigationStore);
            _modalNavigationStore.CurrentViewModel = addClientViewModel;
        }
    }
}

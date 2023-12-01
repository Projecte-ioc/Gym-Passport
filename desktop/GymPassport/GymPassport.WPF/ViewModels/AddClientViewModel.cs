using GymPassport.WPF.Commands;
using GymPassport.WPF.State;
using GymPassport.WPF.State.Navigators;
using System.Windows.Input;

namespace GymPassport.WPF.ViewModels
{
    public class AddClientViewModel : ViewModelBase
    {
        public ClientDetailsFormViewModel ClientDetailsFormViewModel { get; }

        public AddClientViewModel(ClientsStore clientsStore, ModalNavigationStore modalNavigationStore)
        {
            ICommand submitCommand = new AddClientCommand(this, clientsStore, modalNavigationStore);
            ICommand cancelCommand = new CloseModalCommand(modalNavigationStore);
            ClientDetailsFormViewModel = new ClientDetailsFormViewModel(submitCommand, cancelCommand);
        }
    }
}

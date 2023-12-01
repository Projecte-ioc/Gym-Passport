using GymPassport.Domain.Models;
using GymPassport.WPF.Commands;
using GymPassport.WPF.State;
using GymPassport.WPF.State.Navigators;
using System.Windows.Input;

namespace GymPassport.WPF.ViewModels
{
    public class EditClientViewModel : ViewModelBase
    {
        public String ClientUsername { get; }

        public ClientDetailsFormViewModel ClientDetailsFormViewModel { get; }

        public EditClientViewModel(Client client, ClientsStore _clientsStore, ModalNavigationStore modalNavigationStore)
        {
            ClientUsername = client.Username;

            ICommand submitCommand = new EditClientCommand(this, _clientsStore, modalNavigationStore);
            ICommand cancelCommand = new CloseModalCommand(modalNavigationStore);

            ClientDetailsFormViewModel = new ClientDetailsFormViewModel(submitCommand, cancelCommand)
            {
                Name = client.Name,
                Role = client.Role,
                Username = client.Username,
                Password = client.Password
            };
        }
    }
}

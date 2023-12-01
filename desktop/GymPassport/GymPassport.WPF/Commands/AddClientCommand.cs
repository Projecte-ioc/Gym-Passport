using GymPassport.Domain.Models;
using GymPassport.WPF.State;
using GymPassport.WPF.State.Navigators;
using GymPassport.WPF.ViewModels;

namespace GymPassport.WPF.Commands
{
    public class AddClientCommand : AsyncCommandBase
    {
        private readonly AddClientViewModel _addClientViewModel;
        private readonly ClientsStore _clientsStore;
        private readonly ModalNavigationStore _modalNavigationStore;

        public AddClientCommand(AddClientViewModel addClientViewModel, ClientsStore clientsStore, ModalNavigationStore modalNavigationStore)
        {
            _addClientViewModel = addClientViewModel;
            _clientsStore = clientsStore;
            _modalNavigationStore = modalNavigationStore;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            ClientDetailsFormViewModel formViewModel = _addClientViewModel.ClientDetailsFormViewModel;

            formViewModel.ErrorMessage = null;
            formViewModel.IsSubmitting = true;

            Client client = new Client(
                formViewModel.Name,
                formViewModel.Role,
                formViewModel.Username,
                formViewModel.Password);

            try
            {
                await _clientsStore.Add(client);

                _modalNavigationStore.Close();
            }
            catch (Exception)
            {
                formViewModel.ErrorMessage = "Error al añadir al cliente. Por favor, intentelo de nuevo.";
            }
            finally
            {
                formViewModel.IsSubmitting = false;
            }
        }
    }
}

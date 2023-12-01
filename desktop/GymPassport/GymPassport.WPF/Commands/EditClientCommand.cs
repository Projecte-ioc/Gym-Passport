using GymPassport.Domain.Models;
using GymPassport.WPF.State;
using GymPassport.WPF.State.Navigators;
using GymPassport.WPF.ViewModels;

namespace GymPassport.WPF.Commands
{
    public class EditClientCommand : AsyncCommandBase
    {
        private readonly EditClientViewModel _editClientViewModel;
        private readonly ClientsStore _clientsStore;
        private readonly ModalNavigationStore _modalNavigationStore;

        public EditClientCommand(EditClientViewModel editClientViewModel, ClientsStore clientsStore, ModalNavigationStore modalNavigationStore)
        {
            _editClientViewModel = editClientViewModel;
            _clientsStore = clientsStore;
            _modalNavigationStore = modalNavigationStore;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            ClientDetailsFormViewModel formViewModel = _editClientViewModel.ClientDetailsFormViewModel;

            formViewModel.ErrorMessage = null;
            formViewModel.IsSubmitting = true;

            Client client = new Client(
                formViewModel.Name,
                formViewModel.Role,
                _editClientViewModel.ClientUsername,
                formViewModel.Password);

            try
            {
                await _clientsStore.Update(client);

                _modalNavigationStore.Close();
            }
            catch (Exception)
            {
                formViewModel.ErrorMessage = "Error al actualizar el cliente. Por favor, vuelva a intentarlo.";
            }
            finally
            {
                formViewModel.IsSubmitting = false;
            }
        }
    }
}

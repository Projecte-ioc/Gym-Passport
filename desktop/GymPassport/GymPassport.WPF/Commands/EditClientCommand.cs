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
            _editClientViewModel.ErrorMessage = null;
            _editClientViewModel.IsSubmitting = true;

            Client client = new Client(
                _editClientViewModel.Name,
                _editClientViewModel.Role,
                _editClientViewModel.ClientUsername,
                _editClientViewModel.Password);

            try
            {
                await _clientsStore.Update(client);

                _modalNavigationStore.Close();
            }
            catch (Exception)
            {
                _editClientViewModel.ErrorMessage = "Error al actualizar el cliente. Por favor, vuelva a intentarlo.";
            }
            finally
            {
                _editClientViewModel.IsSubmitting = false;
            }
        }
    }
}

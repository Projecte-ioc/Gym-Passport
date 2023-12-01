using GymPassport.Domain.Models;
using GymPassport.WPF.State;
using GymPassport.WPF.ViewModels;

namespace GymPassport.WPF.Commands
{
    public class DeleteClientCommand : AsyncCommandBase
    {
        private readonly ClientsListingItemViewModel _clientsListingItemViewModel;
        private readonly ClientsStore _clientsStore;

        public DeleteClientCommand(ClientsListingItemViewModel clientsListingItemViewModel,
            ClientsStore clientsStore)
        {
            _clientsListingItemViewModel = clientsListingItemViewModel;
            _clientsStore = clientsStore;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _clientsListingItemViewModel.ErrorMessage = null;
            _clientsListingItemViewModel.IsDeleting = true;

            Client client = _clientsListingItemViewModel.Client;

            try
            {
                await _clientsStore.Delete(client.Username);
            }
            catch (Exception)
            {
                _clientsListingItemViewModel.ErrorMessage = "Error al eliminar al cliente. Por favor, intentelo de nuevo.";
            }
            finally
            {
                _clientsListingItemViewModel.IsDeleting = false;
            }
        }
    }
}

using GymPassport.WPF.State.Clients;
using GymPassport.WPF.ViewModels;

namespace GymPassport.WPF.Commands
{
    public class LoadClientsCommand : AsyncCommandBase
    {
        private readonly ClientsViewModel _clientsViewModel;
        private readonly ClientsStore _clientsStore;

        public LoadClientsCommand(ClientsViewModel clientsViewModel, ClientsStore clientsStore)
        {
            _clientsViewModel = clientsViewModel;
            _clientsStore = clientsStore;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _clientsViewModel.ErrorMessage = null;
            _clientsViewModel.IsLoading = true;

            try
            {
                await _clientsStore.Load();
            }
            catch (Exception)
            {
                _clientsViewModel.ErrorMessage = "Error al cargar los clientes. Por favor, intentelo de nuevo.";
            }
            finally
            {
                _clientsViewModel.IsLoading = false;
            }
        }
    }
}

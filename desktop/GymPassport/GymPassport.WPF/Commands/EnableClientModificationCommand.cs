using GymPassport.WPF.ViewModels;

namespace GymPassport.WPF.Commands
{
    public class EnableClientModificationCommand : CommandBase
    {
        private readonly ClientsViewModel _clientsViewModel;

        public EnableClientModificationCommand(ClientsViewModel clientsViewModel)
        {
            _clientsViewModel = clientsViewModel;
        }

        public override void Execute(object? parameter)
        {
            if (!_clientsViewModel.IsEnabled)
            {
                _clientsViewModel.IsEnabled = true;
                _clientsViewModel.btnToggleModificationText = "Bloquejar modificació";
            }
            else
            {
                _clientsViewModel.IsEnabled = false;
                _clientsViewModel.btnToggleModificationText = "Desbloquejar modificació";
            }
        }
    }
}

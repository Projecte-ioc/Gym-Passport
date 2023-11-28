using GymPassport.WPF.ViewModels;

namespace GymPassport.WPF.Commands
{
    public class GetClientCommand : CommandBase
    {
        private readonly ClientsViewModel _clientsViewModel;

        public GetClientCommand(ClientsViewModel clientsViewModel)
        {
            _clientsViewModel = clientsViewModel;
        }

        public override void Execute(object? parameter)
        {
            //_clientsViewModel.CurrentClient = (Client)parameter;
            if (parameter != null)
            {
                ClientViewModel client = (ClientViewModel)parameter;
                _clientsViewModel.CurrentClient = (ClientViewModel)client.Clone();
            }
        }
    }
}

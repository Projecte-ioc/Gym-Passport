using GymPassport.Domain.Models;
using GymPassport.WPF.State;

namespace GymPassport.WPF.ViewModels
{
    public class ClientsDetailsViewModel : ViewModelBase
    {
        private readonly SelectedClientStore _selectedClientStore;

        private Client SelectedClient => _selectedClientStore.SelectedClient;

        public bool HasSelectedClient => SelectedClient != null;
        public string Name => SelectedClient?.Name ?? "Desconocido";
        public string Role => SelectedClient?.Role ?? "Desconocido";
        public string Username => SelectedClient?.Username ?? "Desconocido";
        public string Password => SelectedClient?.Password ?? "Desconocido";

        public ClientsDetailsViewModel(SelectedClientStore selectedClientStore)
        {
            _selectedClientStore = selectedClientStore;
            _selectedClientStore.SelectedClientChanged += SelectedClientStore_SelectedClientChanged;
        }

        protected override void Dispose()
        {
            _selectedClientStore.SelectedClientChanged -= SelectedClientStore_SelectedClientChanged;
            base.Dispose();
        }

        private void SelectedClientStore_SelectedClientChanged()
        {
            OnPropertyChanged(nameof(HasSelectedClient));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Role));
            OnPropertyChanged(nameof(Username));
            OnPropertyChanged(nameof(Password));
        }
    }
}

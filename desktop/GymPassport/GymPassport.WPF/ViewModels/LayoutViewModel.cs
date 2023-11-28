using GymPassport.WPF.State.Accounts;

namespace GymPassport.WPF.ViewModels
{
    public class LayoutViewModel : ViewModelBase
    {
        // Campos
        private readonly IAccountStore _accountStore;
        private string _displayWelcomeMessage;
        private string _displayUserRole;
        private string _caption;

        // Propiedades
        public string DisplayWelcomeMessage
        {
            get
            {
                return _displayWelcomeMessage;
            }
            set
            {
                _displayWelcomeMessage = value;
            }
        }

        public string DisplayUserRole
        {
            get
            {
                return _displayUserRole;
            }
            set
            {
                _displayUserRole = value;
            }
        }

        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }

        public NavigationBarViewModel NavigationBarViewModel { get; }
        public ViewModelBase ContentViewModel { get; }

        public LayoutViewModel(IAccountStore accountStore, NavigationBarViewModel navigationBarViewModel, ViewModelBase contentViewModel)
        {
            _accountStore = accountStore;

            if (_accountStore.CurrentAccount.Role.Equals("normal"))
            {
                DisplayUserRole = "Usuari corrent";
            }

            if (_accountStore.CurrentAccount.Role.Equals("admin"))
            {
                DisplayUserRole = "Administrador";
            }

            _displayWelcomeMessage = $"Benvingut a Gym Passport, {_accountStore.CurrentAccount.Name}";

            NavigationBarViewModel = navigationBarViewModel;
            ContentViewModel = contentViewModel;
        }

        public override void Dispose()
        {
            NavigationBarViewModel.Dispose();
            ContentViewModel.Dispose();
            base.Dispose();
        }
    }
}

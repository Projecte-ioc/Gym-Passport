using System.Windows.Input;
using Gym_Passport.Models;

namespace Gym_Passport.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        //Campos
        private UserAccountModel _currentUserAccount;
        private ViewModelBase _currentChildView;
        private string _caption;
        private string _displayWelcomeMessage;
        private string _displayUserRole;

        //Propiedades
        public UserAccountModel CurrentUserAccount 
        {
            get
            {
                return _currentUserAccount;
            }
            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
            }
        }

        public ViewModelBase CurrentChildView 
        { 
            get
            {
                return _currentChildView;
            }

            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
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

        //--> Comandos
        public ICommand ShowActivityViewCommand { get; }
        public ICommand ShowReservationViewCommand { get; }
        public ICommand ShowRoomViewCommand { get; }

        public MainViewModel(UserAccountModel currentAccount)
        {
            CurrentUserAccount = currentAccount;

            if (CurrentUserAccount.Role.Equals("normal"))
            {
                DisplayUserRole = "Usuari corrent";
            }

            if (CurrentUserAccount.Role.Equals("admin"))
            {
                DisplayUserRole = "Administrador";
            }

            _displayWelcomeMessage = $"Benvingut a Gym Passport, {CurrentUserAccount.Name}";

            //Inicialización de comandos
            ShowActivityViewCommand = new ViewModelCommand(ExecuteShowActivityViewCommand);
            ShowReservationViewCommand = new ViewModelCommand(ExecuteShowReservationViewCommand);
            ShowRoomViewCommand = new ViewModelCommand(ExecuteShowRoomViewCommand);

            //Vista por defecto
            ExecuteShowActivityViewCommand(null);
        }

        /// <summary>
        /// Método que cambia el CurrentChildView al UserControl 'ActivityView'.
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteShowActivityViewCommand(object obj)
        {
            CurrentChildView = new ActivityViewModel();
            Caption = "Activitats";
        }

        /// <summary>
        /// Método que cambia el CurrentChildView al UserControl 'ReservationView'.
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteShowReservationViewCommand(object obj)
        {
            CurrentChildView = new ReservationViewModel();
            Caption = "Reserves";
        }

        /// <summary>
        /// Método que cambia el CurrentChildView al UserControl 'RoomView'.
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteShowRoomViewCommand(object obj)
        {
            CurrentChildView = new RoomViewModel();
            Caption = "Espais";
        }
    }
}

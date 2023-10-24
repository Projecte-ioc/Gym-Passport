using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FontAwesome.Sharp;
using Gym_Passport.Models;
using Gym_Passport.Repositories;

namespace Gym_Passport.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        //Campos
        private UserAccountModel _currentUserAccount;
        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;

        private IUserRepository userRepository;

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
        public IconChar Icon 
        {
            get
            {
                return _icon;
            }

            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

        //--> Comandos
        public ICommand ShowActivityViewCommand { get; }
        public ICommand ShowReservationViewCommand { get; }
        public ICommand ShowRoomViewCommand { get; }

        public MainViewModel()
        {
            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();

            //Inicialización de comandos
            ShowActivityViewCommand = new ViewModelCommand(ExecuteShowActivityViewCommand);
            ShowReservationViewCommand = new ViewModelCommand(ExecuteShowReservationViewCommand);
            ShowRoomViewCommand = new ViewModelCommand(ExecuteShowRoomViewCommand);

            //Vista por defecto

            ExecuteShowActivityViewCommand(null);
            LoadCurrentUserData();
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

        /// <summary>
        /// Método que actualiza los datos de la cuenta logueada (CurrentUserAccount)
        /// </summary>
        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if(user != null)
            {
                CurrentUserAccount.Username = user.Username;
                CurrentUserAccount.DisplayName = $"Benvingut/da {user.Name}";
                if(user.Role == "admin")
                {
                    CurrentUserAccount.Role = "Administrador";
                }
                if (user.Role == "user")
                {
                    CurrentUserAccount.Role = "Usuari";
                }
            } 
            else
            {
                CurrentUserAccount.DisplayName = "Usuari invàlid. No s'ha pogut iniciar sessió.";
            }
        }
    }
}

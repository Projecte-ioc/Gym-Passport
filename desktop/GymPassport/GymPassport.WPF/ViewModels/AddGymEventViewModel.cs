using GymPassport.WPF.Commands;
using GymPassport.WPF.State.GymEvents;
using GymPassport.WPF.State.Navigators;
using System.Windows.Input;

namespace GymPassport.WPF.ViewModels
{
    public class AddGymEventViewModel : ViewModelBase
    {
        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
                OnPropertyChanged(nameof(CanSubmit));
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _location;
        public string Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        private int _numParticipants;
        public int NumParticipants
        {
            get
            {
                return _numParticipants;
            }
            set
            {
                _numParticipants = value;
                OnPropertyChanged(nameof(NumParticipants));
            }
        }

        private int _numAttendances;
        public int NumAttendances
        {
            get
            {
                return _numAttendances;
            }
            set
            {
                _numAttendances = value;
                OnPropertyChanged(nameof(NumAttendances));
            }
        }

        private int _clientId;
        public int ClientId
        {
            get
            {
                return _clientId;
            }
            set
            {
                _clientId = value;
                OnPropertyChanged(nameof(ClientId));
            }
        }

        private int _gymId;
        public int GymId
        {
            get
            {
                return _gymId;
            }
            set
            {
                _gymId = value;
                OnPropertyChanged(nameof(GymId));
            }
        }

        private bool _done;
        public bool Done
        {
            get
            {
                return _done;
            }
            set
            {
                _done = value;
                OnPropertyChanged(nameof(Done));
            }
        }

        private string _date;
        public string Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        private int _hour;
        public int Hour
        {
            get
            {
                return _hour;
            }
            set
            {
                _hour = value;
                OnPropertyChanged(nameof(Hour));
            }
        }

        private int _minute;
        public int Minute
        {
            get
            {
                return _minute;
            }
            set
            {
                _minute = value;
                OnPropertyChanged(nameof(Minute));
            }
        }

        private bool _isSubmitting;
        public bool IsSubmitting
        {
            get
            {
                return _isSubmitting;
            }
            set
            {
                _isSubmitting = value;
                OnPropertyChanged(nameof(IsSubmitting));
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        public bool CanSubmit => !string.IsNullOrEmpty(Id.ToString());

        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        public AddGymEventViewModel(GymEventsStore gymEventsStore, ModalNavigationStore modalNavigationStore)
        {
            SubmitCommand = new AddGymEventCommand(this, gymEventsStore, modalNavigationStore);
            CancelCommand = new CloseModalCommand(modalNavigationStore);
        }
    }
}

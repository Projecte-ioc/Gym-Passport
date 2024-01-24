using GymPassport.Domain.Models;
using GymPassport.WPF.Commands;
using GymPassport.WPF.State.GymEvents;
using GymPassport.WPF.State.Navigators;
using System.Windows.Input;

namespace GymPassport.WPF.ViewModels
{
    public class GymEventsListingItemViewModel : ViewModelBase
    {
        public GymEvent GymEvent { get; private set; }

        public string Name => GymEvent.Name;

        private bool _isDeleting;
        public bool IsDeleting
        {
            get
            {
                return _isDeleting;
            }
            set
            {
                _isDeleting = value;
                OnPropertyChanged(nameof(IsDeleting));
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

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public GymEventsListingItemViewModel(GymEvent gymEvent, GymEventsStore gymEventsStore, ModalNavigationStore modalNavigationStore)
        {
            GymEvent = gymEvent;

            EditCommand = new OpenEditGymEventCommand(this, gymEventsStore, modalNavigationStore);
            DeleteCommand = new DeleteGymEventCommand(this, gymEventsStore);
        }

        public void Update(GymEvent gymEvent)
        {
            GymEvent = gymEvent;
            OnPropertyChanged(nameof(Name));
        }
    }
}

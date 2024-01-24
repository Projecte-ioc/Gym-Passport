using GymPassport.WPF.Commands;
using GymPassport.WPF.State.GymEvents;
using GymPassport.WPF.State.Navigators;
using System.Windows.Input;

namespace GymPassport.WPF.ViewModels
{
    public class GymEventsViewModel : ViewModelBase
    {
        public GymEventsListingViewModel GymEventsListingViewModel { get; }
        public GymEventsDetailsViewModel GymEventsDetailsViewModel { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
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

        public ICommand LoadGymEventsCommand { get; }
        public ICommand AddGymEventsCommand { get; }

        public GymEventsViewModel(GymEventsStore gymEventsStore, SelectedGymEventStore _selectedGymEventStore, ModalNavigationStore modalNavigationStore)
        {
            GymEventsListingViewModel = new GymEventsListingViewModel(gymEventsStore, _selectedGymEventStore, modalNavigationStore);
            GymEventsDetailsViewModel = new GymEventsDetailsViewModel(_selectedGymEventStore);

            LoadGymEventsCommand = new LoadGymEventsCommand(this, gymEventsStore);
            AddGymEventsCommand = new OpenAddGymEventCommand(gymEventsStore, modalNavigationStore);
        }

        public static GymEventsViewModel LoadViewModel(GymEventsStore gymEventsStore, SelectedGymEventStore selectedGymEventStore, ModalNavigationStore modalNavigationStore)
        {
            GymEventsViewModel viewModel = new GymEventsViewModel(gymEventsStore, selectedGymEventStore, modalNavigationStore);
            viewModel.LoadGymEventsCommand.Execute(null);
            return viewModel;
        }
    }
}

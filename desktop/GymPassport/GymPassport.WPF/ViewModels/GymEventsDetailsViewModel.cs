using GymPassport.Domain.Models;
using GymPassport.WPF.State.GymEvents;

namespace GymPassport.WPF.ViewModels
{
    public class GymEventsDetailsViewModel : ViewModelBase
    {
        private readonly SelectedGymEventStore _selectedGymEventStore;

        private GymEvent SelectedGymEvent => _selectedGymEventStore.SelectedGymEvent;

        public bool HasSelectedGymEvent => SelectedGymEvent != null;
        public int Id => SelectedGymEvent?.Id ?? -1;
        public string Name => SelectedGymEvent?.Name ?? "Desconocido";
        public string Location => SelectedGymEvent?.Location ?? "Desconocido";
        public int NumParticipants => SelectedGymEvent?.NumParticipants ?? -1;
        public int NumAttendances => SelectedGymEvent?.NumAttendances ?? -1;
        public int ClientId => SelectedGymEvent?.ClientId ?? -1;
        public int GymId => SelectedGymEvent?.GymId ?? -1;
        public bool Done => SelectedGymEvent?.Done ?? false;
        public string Date => SelectedGymEvent?.Date ?? "Desconocido";
        public int Hour => SelectedGymEvent?.Hour ?? -1;
        public int Minute => SelectedGymEvent?.Minute ?? -1;

        public GymEventsDetailsViewModel(SelectedGymEventStore selectedGymEventStore)
        {
            _selectedGymEventStore = selectedGymEventStore;
            _selectedGymEventStore.SelectedGymEventChanged += SelectedGymEventStore_SelectedGymEventChanged;
        }

        protected override void Dispose()
        {
            _selectedGymEventStore.SelectedGymEventChanged -= SelectedGymEventStore_SelectedGymEventChanged;
            base.Dispose();
        }

        private void SelectedGymEventStore_SelectedGymEventChanged()
        {
            OnPropertyChanged(nameof(HasSelectedGymEvent));
            OnPropertyChanged(nameof(Id));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Location));
            OnPropertyChanged(nameof(NumParticipants));
            OnPropertyChanged(nameof(NumAttendances));
            OnPropertyChanged(nameof(ClientId));
            OnPropertyChanged(nameof(GymId));
            OnPropertyChanged(nameof(Done));
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(Hour));
            OnPropertyChanged(nameof(Minute));
        }
    }
}

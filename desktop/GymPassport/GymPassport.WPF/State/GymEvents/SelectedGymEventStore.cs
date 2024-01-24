using GymPassport.Domain.Models;
using GymPassport.WPF.State.GymEvents;

namespace GymPassport.WPF.State.GymEvents
{
    public class SelectedGymEventStore
    {
        private readonly GymEventsStore _gymEventsStore;

        private GymEvent _selectedGymEvent;
        public GymEvent SelectedGymEvent
        {
            get
            {
                return _selectedGymEvent;
            }
            set
            {
                _selectedGymEvent = value;
                SelectedGymEventChanged?.Invoke();
            }
        }

        public event Action SelectedGymEventChanged;

        public SelectedGymEventStore(GymEventsStore gymEventsStore)
        {
            _gymEventsStore = gymEventsStore;
            _gymEventsStore.GymEventUpdated += GymEventsStore_GymEventUpdated;
            _gymEventsStore.GymEventAdded += GymEventsStore_GymEventAdded;
        }

        private void GymEventsStore_GymEventAdded(GymEvent gymEvent)
        {
            SelectedGymEvent = gymEvent;
        }

        private void GymEventsStore_GymEventUpdated(GymEvent gymEvent)
        {
            if (gymEvent.Id == SelectedGymEvent?.Id)
            {
                SelectedGymEvent = gymEvent;
            }
        }
    }
}

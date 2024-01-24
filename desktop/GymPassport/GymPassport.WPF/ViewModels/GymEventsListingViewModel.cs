using GymPassport.Domain.Models;
using GymPassport.WPF.State.GymEvents;
using GymPassport.WPF.State.Navigators;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace GymPassport.WPF.ViewModels
{
    public class GymEventsListingViewModel : ViewModelBase
    {
        private readonly GymEventsStore _gymEventsStore;
        private readonly SelectedGymEventStore _selectedGymEventStore;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly ObservableCollection<GymEventsListingItemViewModel> _gymEventsListingItemViewModels;

        public IEnumerable<GymEventsListingItemViewModel> GymEventsListingItemViewModels => _gymEventsListingItemViewModels;

        public GymEventsListingItemViewModel SelectedGymEventListingItemViewModel
        {
            get
            {
                return _gymEventsListingItemViewModels
                    .FirstOrDefault(y => y.GymEvent?.Id == _selectedGymEventStore.SelectedGymEvent?.Id);
            }
            set
            {
                _selectedGymEventStore.SelectedGymEvent = value?.GymEvent;
            }
        }

        public GymEventsListingViewModel(GymEventsStore gymEventsStore, SelectedGymEventStore selectedGymEventStore, ModalNavigationStore modalNavigationStore)
        {
            _gymEventsStore = gymEventsStore;
            _selectedGymEventStore = selectedGymEventStore;
            _modalNavigationStore = modalNavigationStore;
            _gymEventsListingItemViewModels = new ObservableCollection<GymEventsListingItemViewModel>();

            _selectedGymEventStore.SelectedGymEventChanged += SelectedGymEventStore_SelectedGymEventChanged;

            _gymEventsStore.GymEventsLoaded += GymEventsStore_GymEventsLoaded;
            _gymEventsStore.GymEventAdded += GymEventsStore_GymEventAdded;
            _gymEventsStore.GymEventUpdated += GymEventsStore_GymEventUpdated;
            _gymEventsStore.GymEventDeleted += GymEventsStore_GymEventDeleted;

            _gymEventsListingItemViewModels.CollectionChanged += GymEventsListingItemViewModels_CollectionChanged;
        }

        protected override void Dispose()
        {
            _gymEventsListingItemViewModels.CollectionChanged -= GymEventsListingItemViewModels_CollectionChanged;
            _selectedGymEventStore.SelectedGymEventChanged -= SelectedGymEventStore_SelectedGymEventChanged;
            _gymEventsStore.GymEventsLoaded -= GymEventsStore_GymEventsLoaded;
            _gymEventsStore.GymEventAdded -= GymEventsStore_GymEventAdded;
            _gymEventsStore.GymEventUpdated -= GymEventsStore_GymEventUpdated;
            _gymEventsStore.GymEventDeleted -= GymEventsStore_GymEventDeleted;

            base.Dispose();
        }

        private void SelectedGymEventStore_SelectedGymEventChanged()
        {
            OnPropertyChanged(nameof(SelectedGymEventListingItemViewModel));
        }

        private void GymEventsStore_GymEventsLoaded()
        {
            _gymEventsListingItemViewModels.Clear();

            foreach (GymEvent gymEvent in _gymEventsStore.GymEvents)
            {
                AddGymEvent(gymEvent);
            }
        }

        private void GymEventsStore_GymEventAdded(GymEvent gymEvent)
        {
            AddGymEvent(gymEvent);
        }

        private void GymEventsStore_GymEventUpdated(GymEvent gymEvent)
        {
            GymEventsListingItemViewModel gymEventViewModel =
                _gymEventsListingItemViewModels.FirstOrDefault(y => y.GymEvent.Id == gymEvent.Id);

            if (gymEventViewModel != null)
            {
                gymEventViewModel.Update(gymEvent);
            }
        }

        private void GymEventsStore_GymEventDeleted(int id)
        {
            GymEventsListingItemViewModel? itemViewModel = _gymEventsListingItemViewModels.FirstOrDefault(y => y.GymEvent?.Id == id);

            if (itemViewModel != null)
            {
                _gymEventsListingItemViewModels.Remove(itemViewModel);
            }
        }

        private void GymEventsListingItemViewModels_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(SelectedGymEventListingItemViewModel));
        }

        private void AddGymEvent(GymEvent gymEvent)
        {
            GymEventsListingItemViewModel itemViewModel =
                new GymEventsListingItemViewModel(gymEvent, _gymEventsStore, _modalNavigationStore);
            _gymEventsListingItemViewModels.Add(itemViewModel);
        }
    }
}

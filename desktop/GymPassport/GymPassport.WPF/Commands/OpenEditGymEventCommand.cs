using GymPassport.Domain.Models;
using GymPassport.WPF.State.GymEvents;
using GymPassport.WPF.State.Navigators;
using GymPassport.WPF.ViewModels;

namespace GymPassport.WPF.Commands
{
    public class OpenEditGymEventCommand : CommandBase
    {
        private readonly GymEventsListingItemViewModel _gymEventsListingItemViewModel;
        private readonly GymEventsStore _gymEventsStore;
        private readonly ModalNavigationStore _modalNavigationStore;

        public OpenEditGymEventCommand(GymEventsListingItemViewModel gymEventsListingItemViewModel,
            GymEventsStore gymEventsStore,
            ModalNavigationStore modalNavigationStore)
        {
            _gymEventsListingItemViewModel = gymEventsListingItemViewModel;
            _gymEventsStore = gymEventsStore;
            _modalNavigationStore = modalNavigationStore;
        }

        public override void Execute(object parameter)
        {
            GymEvent gymEvent = _gymEventsListingItemViewModel.GymEvent;

            EditGymEventViewModel editGymEventViewModel =
                new EditGymEventViewModel(gymEvent, _gymEventsStore, _modalNavigationStore);
            _modalNavigationStore.CurrentViewModel = editGymEventViewModel;
        }
    }
}

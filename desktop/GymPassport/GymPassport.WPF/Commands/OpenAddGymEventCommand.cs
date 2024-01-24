using GymPassport.WPF.State.GymEvents;
using GymPassport.WPF.State.Navigators;
using GymPassport.WPF.ViewModels;

namespace GymPassport.WPF.Commands
{
    public class OpenAddGymEventCommand : CommandBase
    {
        private readonly GymEventsStore _gymEventsStore;
        private readonly ModalNavigationStore _modalNavigationStore;

        public OpenAddGymEventCommand(GymEventsStore gymEventsStore, ModalNavigationStore modalNavigationStore)
        {
            _gymEventsStore = gymEventsStore;
            _modalNavigationStore = modalNavigationStore;
        }

        public override void Execute(object parameter)
        {
            AddGymEventViewModel addGymEventViewModel = new AddGymEventViewModel(_gymEventsStore, _modalNavigationStore);
            _modalNavigationStore.CurrentViewModel = addGymEventViewModel;
        }
    }
}

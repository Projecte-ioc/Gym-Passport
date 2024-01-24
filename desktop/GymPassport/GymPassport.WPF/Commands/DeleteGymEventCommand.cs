using GymPassport.Domain.Models;
using GymPassport.WPF.State.GymEvents;
using GymPassport.WPF.ViewModels;

namespace GymPassport.WPF.Commands
{
    public class DeleteGymEventCommand : AsyncCommandBase
    {
        private readonly GymEventsListingItemViewModel _gymEventsListingItemViewModel;
        private readonly GymEventsStore _gymEventsStore;

        public DeleteGymEventCommand(GymEventsListingItemViewModel gymEventsListingItemViewModel,
            GymEventsStore gymEventsStore)
        {
            _gymEventsListingItemViewModel = gymEventsListingItemViewModel;
            _gymEventsStore = gymEventsStore;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _gymEventsListingItemViewModel.ErrorMessage = null;
            _gymEventsListingItemViewModel.IsDeleting = true;

            GymEvent gymEvent = _gymEventsListingItemViewModel.GymEvent;

            try
            {
                await _gymEventsStore.Delete(gymEvent.Id);
            }
            catch (Exception)
            {
                _gymEventsListingItemViewModel.ErrorMessage = "Error al eliminar evento. Por favor, intentelo de nuevo.";
            }
            finally
            {
                _gymEventsListingItemViewModel.IsDeleting = false;
            }
        }
    }
}

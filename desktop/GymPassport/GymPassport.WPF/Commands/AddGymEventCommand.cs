using GymPassport.Domain.Models;
using GymPassport.WPF.State.GymEvents;
using GymPassport.WPF.State.Navigators;
using GymPassport.WPF.ViewModels;

namespace GymPassport.WPF.Commands
{
    public class AddGymEventCommand : AsyncCommandBase
    {
        private readonly AddGymEventViewModel _addGymEventViewModel;
        private readonly GymEventsStore _gymEventsStore;
        private readonly ModalNavigationStore _modalNavigationStore;

        public AddGymEventCommand(AddGymEventViewModel addGymEventViewModel, GymEventsStore gymEventsStore, ModalNavigationStore modalNavigationStore)
        {
            _addGymEventViewModel = addGymEventViewModel;
            _gymEventsStore = gymEventsStore;
            _modalNavigationStore = modalNavigationStore;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _addGymEventViewModel.ErrorMessage = null;
            _addGymEventViewModel.IsSubmitting = true;

            GymEvent gymEvent = new GymEvent
            {
                Id = _addGymEventViewModel.Id,
                Name = _addGymEventViewModel.Name,
                Location = _addGymEventViewModel.Location,
                NumParticipants = _addGymEventViewModel.NumParticipants,
                NumAttendances = _addGymEventViewModel.NumAttendances,
                ClientId = _addGymEventViewModel.ClientId,
                GymId = _addGymEventViewModel.GymId,
                Done = _addGymEventViewModel.Done,
                Date = _addGymEventViewModel.Date,
                Hour = _addGymEventViewModel.Hour,
                Minute = _addGymEventViewModel.Minute
            };

            try
            {
                await _gymEventsStore.Add(gymEvent);

                _modalNavigationStore.Close();
            }
            catch (Exception)
            {
                _addGymEventViewModel.ErrorMessage = "Error al añadir al evento. Por favor, intentelo de nuevo.";
            }
            finally
            {
                _addGymEventViewModel.IsSubmitting = false;
            }
        }
    }
}

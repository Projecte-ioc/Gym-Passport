using GymPassport.Domain.Models;
using GymPassport.WPF.State.GymEvents;
using GymPassport.WPF.State.Navigators;
using GymPassport.WPF.ViewModels;

namespace GymPassport.WPF.Commands
{
    public class EditGymEventCommand : AsyncCommandBase
    {
        private readonly EditGymEventViewModel _editGymEventViewModel;
        private readonly GymEventsStore _gymEventsStore;
        private readonly ModalNavigationStore _modalNavigationStore;

        public EditGymEventCommand(EditGymEventViewModel editGymEventViewModel, GymEventsStore gymEventsStore, ModalNavigationStore modalNavigationStore)
        {
            _editGymEventViewModel = editGymEventViewModel;
            _gymEventsStore = gymEventsStore;
            _modalNavigationStore = modalNavigationStore;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _editGymEventViewModel.ErrorMessage = null;
            _editGymEventViewModel.IsSubmitting = true;

            GymEvent gymEvent = new GymEvent
            {
                Id = _editGymEventViewModel.Id,
                Name = _editGymEventViewModel.Name,
                Location = _editGymEventViewModel.Location,
                NumParticipants = _editGymEventViewModel.NumParticipants,
                NumAttendances = _editGymEventViewModel.NumAttendances,
                ClientId = _editGymEventViewModel.ClientId,
                GymId = _editGymEventViewModel.GymId,
                Done = _editGymEventViewModel.Done,
                Date = _editGymEventViewModel.Date,
                Hour = _editGymEventViewModel.Hour,
                Minute = _editGymEventViewModel.Minute
            };

            try
            {
                await _gymEventsStore.Update(gymEvent, gymEvent.Id);

                _modalNavigationStore.Close();
            }
            catch (Exception)
            {
                _editGymEventViewModel.ErrorMessage = "Error al actualizar el evento. Por favor, vuelva a intentarlo.";
            }
            finally
            {
                _editGymEventViewModel.IsSubmitting = false;
            }
        }
    }
}

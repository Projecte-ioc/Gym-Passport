using GymPassport.WPF.State.GymEvents;
using GymPassport.WPF.ViewModels;

namespace GymPassport.WPF.Commands
{
    public class LoadGymEventsCommand : AsyncCommandBase
    {
        private readonly GymEventsViewModel _gymEventsViewModel;
        private readonly GymEventsStore _gymEventsStore;

        public LoadGymEventsCommand(GymEventsViewModel gymEventsViewModel, GymEventsStore gymEventsStore)
        {
            _gymEventsViewModel = gymEventsViewModel;
            _gymEventsStore = gymEventsStore;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _gymEventsViewModel.ErrorMessage = null;
            _gymEventsViewModel.IsLoading = true;

            try
            {
                await _gymEventsStore.Load();
            }
            catch (Exception)
            {
                _gymEventsViewModel.ErrorMessage = "Error al cargar los eventos. Por favor, intentelo de nuevo.";
            }
            finally
            {
                _gymEventsViewModel.IsLoading = false;
            }
        }
    }
}

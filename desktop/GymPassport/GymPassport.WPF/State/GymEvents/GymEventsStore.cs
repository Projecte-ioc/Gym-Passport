using GymPassport.Domain.Commands;
using GymPassport.Domain.Models;
using GymPassport.Domain.Queries;
using GymPassport.WPF.State.Accounts;

namespace GymPassport.WPF.State.GymEvents
{
    public class GymEventsStore
    {
        private readonly IAccountStore _accountStore;
        private readonly IGetAllGymEventsQuery _getAllGymEventsQuery;
        private readonly ICreateGymEventCommand _createGymEventCommand;
        private readonly IUpdateGymEventCommand _updateGymEventCommand;
        private readonly IDeleteGymEventCommand _deleteGymEventCommand;

        private readonly List<GymEvent> _gymEvents;
        public IEnumerable<GymEvent> GymEvents => _gymEvents;

        public event Action GymEventsLoaded;
        public event Action<GymEvent> GymEventAdded;
        public event Action<GymEvent> GymEventUpdated;
        public event Action<int> GymEventDeleted;

        public GymEventsStore(IAccountStore accountStore,
            IGetAllGymEventsQuery getAllGymEventsQuery,
            ICreateGymEventCommand createGymEventCommand,
            IUpdateGymEventCommand updateGymEventCommand,
            IDeleteGymEventCommand deleteGymEventCommand)
        {
            _accountStore = accountStore;
            _getAllGymEventsQuery = getAllGymEventsQuery;
            _createGymEventCommand = createGymEventCommand;
            _updateGymEventCommand = updateGymEventCommand;
            _deleteGymEventCommand = deleteGymEventCommand;

            _gymEvents = new List<GymEvent>();
        }

        public async Task Load()
        {
            IEnumerable<GymEvent> gymEvents = await _getAllGymEventsQuery.Execute(_accountStore.CurrentAccount.AuthToken);

            _gymEvents.Clear();
            _gymEvents.AddRange(gymEvents);

            GymEventsLoaded?.Invoke();
        }

        public async Task Add(GymEvent gymEvent)
        {
            await _createGymEventCommand.Execute(_accountStore.CurrentAccount.AuthToken, gymEvent);

            _gymEvents.Add(gymEvent);

            GymEventAdded?.Invoke(gymEvent);
        }

        public async Task Update(GymEvent gymEvent, int id)
        {
            await _updateGymEventCommand.Execute(_accountStore.CurrentAccount.AuthToken, gymEvent, id);

            int currentIndex = _gymEvents.FindIndex(y => y.Id == gymEvent.Id);

            if (currentIndex != -1)
            {
                _gymEvents[currentIndex] = gymEvent;
            }
            else
            {
                _gymEvents.Add(gymEvent);
            }

            GymEventUpdated?.Invoke(gymEvent);
        }

        public async Task Delete(int id)
        {
            await _deleteGymEventCommand.Execute(_accountStore.CurrentAccount.AuthToken, id);

            _gymEvents.RemoveAll(y => y.Id == id);

            GymEventDeleted?.Invoke(id);
        }
    }
}

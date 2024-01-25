using GymPassport.Domain.Commands;
using GymPassport.GymPassportAPI.ApiConnectors;
using GymPassport.GymPassportAPI.Services.GymEventServices;

namespace GymPassport.GymPassportAPI.Commands
{
    public class DeleteGymEventCommand : IDeleteGymEventCommand
    {
        private readonly IGymEventApiConnector _gymEventApiConnector;

        public DeleteGymEventCommand(IGymEventApiConnector gymEventApiConnector)
        {
            _gymEventApiConnector = gymEventApiConnector;
        }

        public async Task Execute(string authToken, int id)
        {
            GymEventService gymEventService = new GymEventService(_gymEventApiConnector);
            await gymEventService.DeleteGymEvent(authToken, id);
        }
    }
}

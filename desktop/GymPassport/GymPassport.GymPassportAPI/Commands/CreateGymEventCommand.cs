using GymPassport.Domain.Commands;
using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.ApiConnectors;
using GymPassport.GymPassportAPI.Services.GymEventServices;

namespace GymPassport.GymPassportAPI.Commands
{
    public class CreateGymEventCommand : ICreateGymEventCommand
    {
        private readonly GymEventApiConnector _gymEventApiConnector;

        public CreateGymEventCommand(GymEventApiConnector gymEventApiConnector)
        {
            _gymEventApiConnector = gymEventApiConnector;
        }

        public async Task Execute(string authToken, GymEvent newGymEvent)
        {
            GymEventService gymEventService = new GymEventService(_gymEventApiConnector);
            await gymEventService.InsertGymEvent(authToken, newGymEvent);
        }
    }
}

using GymPassport.Domain.Commands;
using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.ApiConnectors;
using GymPassport.GymPassportAPI.Services.GymEventServices;

namespace GymPassport.GymPassportAPI.Commands
{
    public class UpdateGymEventCommand : IUpdateGymEventCommand
    {
        private readonly IGymEventApiConnector _gymEventApiConnector;

        public UpdateGymEventCommand(IGymEventApiConnector gymEventApiConnector)
        {
            _gymEventApiConnector = gymEventApiConnector;
        }

        public async Task Execute(string authToken, GymEvent updatedGymEvent, int id)
        {
            GymEventService gymEventService = new GymEventService(_gymEventApiConnector);
            await gymEventService.UpdateGymEvent(authToken, updatedGymEvent, id);
        }
    }
}

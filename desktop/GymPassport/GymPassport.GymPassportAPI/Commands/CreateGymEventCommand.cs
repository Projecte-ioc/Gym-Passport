using GymPassport.Domain.Commands;
using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.Services.GymEventServices;

namespace GymPassport.GymPassportAPI.Commands
{
    public class CreateGymEventCommand : ICreateGymEventCommand
    {
        private readonly IGymEventService _gymEventService;

        public CreateGymEventCommand(IGymEventService gymEventService)
        {
            _gymEventService = gymEventService;
        }

        public async Task Execute(string accessToken, GymEvent newGymEvent)
        {
            await _gymEventService.InsertGymEvent(accessToken, newGymEvent);
        }
    }
}

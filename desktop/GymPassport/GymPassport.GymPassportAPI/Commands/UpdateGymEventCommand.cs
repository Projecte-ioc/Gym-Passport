using GymPassport.Domain.Commands;
using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.Services.GymEventServices;

namespace GymPassport.GymPassportAPI.Commands
{
    public class UpdateGymEventCommand : IUpdateGymEventCommand
    {
        private readonly IGymEventService _gymEventService;

        public UpdateGymEventCommand(IGymEventService gymEventService)
        {
            _gymEventService = gymEventService;
        }

        public async Task Execute(string accessToken, GymEvent updatedGymEvent, int id)
        {
            await _gymEventService.UpdateGymEvent(accessToken, updatedGymEvent, id);
        }
    }
}

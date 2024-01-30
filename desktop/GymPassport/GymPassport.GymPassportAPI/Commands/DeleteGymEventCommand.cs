using GymPassport.Domain.Commands;
using GymPassport.GymPassportAPI.Services.GymEventServices;

namespace GymPassport.GymPassportAPI.Commands
{
    public class DeleteGymEventCommand : IDeleteGymEventCommand
    {
        private readonly IGymEventService _gymEventService;

        public DeleteGymEventCommand(IGymEventService gymEventService)
        {
            _gymEventService = gymEventService;
        }

        public async Task Execute(string accessToken, int id)
        {
            await _gymEventService.DeleteGymEvent(accessToken, id);
        }
    }
}

using GymPassport.Domain.Models;
using GymPassport.Domain.Queries;
using GymPassport.GymPassportAPI.Services.GymEventServices;

namespace GymPassport.GymPassportAPI.Queries
{
    public class GetAllGymEventsQuery : IGetAllGymEventsQuery
    {
        private readonly IGymEventService _gymEventService;

        public GetAllGymEventsQuery(IGymEventService gymEventService)
        {
            _gymEventService = gymEventService;
        }

        public async Task<IEnumerable<GymEvent>> Execute(string accessToken)
        {
            return await _gymEventService.GetAllGymEvents(accessToken);
        }
    }
}

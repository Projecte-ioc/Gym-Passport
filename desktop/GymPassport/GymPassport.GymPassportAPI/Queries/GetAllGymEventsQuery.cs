using GymPassport.Domain.Models;
using GymPassport.Domain.Queries;
using GymPassport.GymPassportAPI.ApiConnectors;
using GymPassport.GymPassportAPI.Services.GymEventServices;

namespace GymPassport.GymPassportAPI.Queries
{
    public class GetAllGymEventsQuery : IGetAllGymEventsQuery
    {
        private readonly IGymEventApiConnector _gymEventApiConnector;

        public GetAllGymEventsQuery(IGymEventApiConnector gymEventApiConnector)
        {
            _gymEventApiConnector = gymEventApiConnector;
        }

        public async Task<IEnumerable<GymEvent>> Execute(string accessToken)
        {
            GymEventService gymEventService = new GymEventService(_gymEventApiConnector);
            return await gymEventService.GetAllGymEvents(accessToken);
        }
    }
}

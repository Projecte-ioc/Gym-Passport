using GymPassport.Domain.Models;
using GymPassport.Domain.Queries;
using GymPassport.GymPassportAPI.ApiConnectors;
using GymPassport.GymPassportAPI.Services.GymEventServices;

namespace GymPassport.GymPassportAPI.Queries
{
    public class GetAllGymEventsQuery : IGetAllGymEventsQuery
    {
        private readonly GymEventApiConnector _gymEventApiConnector;

        public GetAllGymEventsQuery(GymEventApiConnector gymEventApiConnector)
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

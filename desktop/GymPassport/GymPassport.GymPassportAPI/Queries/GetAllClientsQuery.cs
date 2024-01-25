using GymPassport.Domain.Models;
using GymPassport.Domain.Queries;
using GymPassport.GymPassportAPI.ApiConnectors;
using GymPassport.GymPassportAPI.Services.GymServices;

namespace GymPassport.GymPassportAPI.Queries
{
    public class GetAllClientsQuery : IGetAllClientsQuery
    {
        private readonly IGymApiConnector _gymApiConnector;

        public GetAllClientsQuery(IGymApiConnector gymApiConnector)
        {
            _gymApiConnector = gymApiConnector;
        }

        public async Task<IEnumerable<Client>> Execute(string accessToken)
        {
            GymService gymService = new GymService(_gymApiConnector);
            return await gymService.GetAllGymClients(accessToken);
        }
    }
}

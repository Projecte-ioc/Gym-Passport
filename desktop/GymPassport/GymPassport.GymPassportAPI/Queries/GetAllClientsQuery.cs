using GymPassport.Domain.Models;
using GymPassport.Domain.Queries;
using GymPassport.GymPassportAPI.Services.GymServices;

namespace GymPassport.GymPassportAPI.Queries
{
    public class GetAllClientsQuery : IGetAllClientsQuery
    {
        private readonly IGymService _gymService;

        public GetAllClientsQuery(IGymService gymService)
        {
            _gymService = gymService;
        }

        public async Task<IEnumerable<Client>> Execute(string accessToken)
        {
            return await _gymService.GetAllGymClients(accessToken);
        }
    }
}

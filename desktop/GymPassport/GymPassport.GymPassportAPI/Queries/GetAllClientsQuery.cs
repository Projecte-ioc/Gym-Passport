using GymPassport.Domain.Models;
using GymPassport.Domain.Queries;
using GymPassport.GymPassportAPI.Services.GymServices;

namespace GymPassport.GymPassportAPI.Queries
{
    public class GetAllClientsQuery : IGetAllClientsQuery
    {
        public async Task<IEnumerable<Client>> Execute(String accessToken)
        {
            return await new GymService().GetAllGymClients(accessToken);
        }
    }
}

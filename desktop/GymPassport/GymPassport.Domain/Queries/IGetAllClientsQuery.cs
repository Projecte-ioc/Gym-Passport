using GymPassport.Domain.Models;

namespace GymPassport.Domain.Queries
{
    public interface IGetAllClientsQuery
    {
        Task<IEnumerable<Client>> Execute(string accessToken);
    }
}
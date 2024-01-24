using GymPassport.Domain.Models;

namespace GymPassport.Domain.Queries
{
    public interface IGetAllGymEventsQuery
    {
        Task<IEnumerable<GymEvent>> Execute(string accessToken);
    }
}
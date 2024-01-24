using GymPassport.Domain.Models;

namespace GymPassport.Domain.Commands
{
    public interface ICreateGymEventCommand
    {
        Task Execute(string authToken, GymEvent newGymEvent);
    }
}
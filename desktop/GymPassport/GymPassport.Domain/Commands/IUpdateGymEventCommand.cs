using GymPassport.Domain.Models;

namespace GymPassport.Domain.Commands
{
    public interface IUpdateGymEventCommand
    {
        Task Execute(string authToken, GymEvent updatedGymEvent, int id);
    }
}
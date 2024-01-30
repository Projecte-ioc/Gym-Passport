using GymPassport.Domain.Models;

namespace GymPassport.Domain.Commands
{
    public interface IUpdateGymEventCommand
    {
        Task Execute(string accessToken, GymEvent updatedGymEvent, int id);
    }
}
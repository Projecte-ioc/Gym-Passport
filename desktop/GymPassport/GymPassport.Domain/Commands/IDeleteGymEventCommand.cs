
namespace GymPassport.Domain.Commands
{
    public interface IDeleteGymEventCommand
    {
        Task Execute(string authToken, int id);
    }
}
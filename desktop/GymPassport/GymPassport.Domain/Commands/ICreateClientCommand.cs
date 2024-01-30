using GymPassport.Domain.Models;

namespace GymPassport.Domain.Commands
{
    public interface ICreateClientCommand
    {
        Task Execute(string accessToken, Client newClient);
    }
}
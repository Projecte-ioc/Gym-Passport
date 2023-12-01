using GymPassport.Domain.Models;

namespace GymPassport.Domain.Commands
{
    public interface ICreateClientCommand
    {
        Task Execute(String accessToken, Client client);
    }
}

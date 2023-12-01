using GymPassport.Domain.Models;

namespace GymPassport.Domain.Commands
{
    public interface IUpdateClientCommand
    {
        Task Execute(string accessToken, Client client);
    }
}

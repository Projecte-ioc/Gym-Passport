namespace GymPassport.Domain.Commands
{
    public interface IDeleteClientCommand
    {
        Task Execute(string authToken, string username);
    }
}

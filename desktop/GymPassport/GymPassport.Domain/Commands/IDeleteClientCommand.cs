namespace GymPassport.Domain.Commands
{
    public interface IDeleteClientCommand
    {
        Task Execute(string accessToken, string username);
    }
}
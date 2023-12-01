using GymPassport.Domain.Commands;
using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.Services.ClientServices;

namespace GymPassport.GymPassportAPI.Commands
{
    public class DeleteClientCommand : IDeleteClientCommand
    {
        public async Task Execute(string accessToken, string username)
        {
            await new ClientService().DeleteClient(accessToken, username);
        }
    }
}

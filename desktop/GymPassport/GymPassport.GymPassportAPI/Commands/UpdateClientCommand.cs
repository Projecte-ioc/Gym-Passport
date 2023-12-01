using GymPassport.Domain.Commands;
using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.Services.ClientServices;

namespace GymPassport.GymPassportAPI.Commands
{
    public class UpdateClientCommand : IUpdateClientCommand
    {
        public async Task Execute(string accessToken, Client client)
        {
            await new ClientService().UpdateClient(accessToken, client);
        }
    }
}

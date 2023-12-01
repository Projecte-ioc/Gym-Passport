using GymPassport.Domain.Commands;
using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.Services.ClientServices;
using GymPassport.GymPassportAPI.Services.GymServices;

namespace GymPassport.GymPassportAPI.Commands
{
    public class CreateClientCommand : ICreateClientCommand
    {
        public async Task Execute(string accessToken, Client client)
        {
            await new ClientService().InsertClient(accessToken, client);
        }
    }
}

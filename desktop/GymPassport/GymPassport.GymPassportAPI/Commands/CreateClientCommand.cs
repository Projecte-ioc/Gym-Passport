using GymPassport.Domain.Commands;
using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.Services.ClientServices;

namespace GymPassport.GymPassportAPI.Commands
{
    public class CreateClientCommand : ICreateClientCommand
    {
        private readonly IClientService _clientService;

        public CreateClientCommand(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task Execute(string accessToken, Client newClient)
        {
            await _clientService.InsertClient(accessToken, newClient);
        }
    }
}

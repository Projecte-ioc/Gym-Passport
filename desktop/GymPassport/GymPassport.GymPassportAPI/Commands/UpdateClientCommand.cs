using GymPassport.Domain.Commands;
using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.Services.ClientServices;

namespace GymPassport.GymPassportAPI.Commands
{
    public class UpdateClientCommand : IUpdateClientCommand
    {
        private readonly IClientService _clientService;

        public UpdateClientCommand(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task Execute(string authToken, Client updatedClient)
        {
            await _clientService.UpdateClient(authToken, updatedClient);
        }
    }
}

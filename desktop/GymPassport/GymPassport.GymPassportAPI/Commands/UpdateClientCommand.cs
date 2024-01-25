using GymPassport.Domain.Commands;
using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.ApiConnectors;
using GymPassport.GymPassportAPI.Services.ClientServices;

namespace GymPassport.GymPassportAPI.Commands
{
    public class UpdateClientCommand : IUpdateClientCommand
    {
        private readonly IClientApiConnector _clientApiConnector;

        public UpdateClientCommand(IClientApiConnector clientApiConnector)
        {
            _clientApiConnector = clientApiConnector;
        }

        public async Task Execute(string authToken, Client updatedClient)
        {
            ClientService clientService = new ClientService(_clientApiConnector);
            await clientService.UpdateClient(authToken, updatedClient);
        }
    }
}

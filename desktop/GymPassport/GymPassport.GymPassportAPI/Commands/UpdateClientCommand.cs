using GymPassport.Domain.Commands;
using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.ApiConnectors;
using GymPassport.GymPassportAPI.Services.ClientServices;

namespace GymPassport.GymPassportAPI.Commands
{
    public class UpdateClientCommand : IUpdateClientCommand
    {
        private readonly ClientApiConnector _clientApiConnector;

        public UpdateClientCommand(ClientApiConnector clientApiConnector)
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

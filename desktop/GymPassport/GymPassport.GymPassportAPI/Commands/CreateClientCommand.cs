using GymPassport.Domain.Commands;
using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.ApiConnectors;
using GymPassport.GymPassportAPI.Services.ClientServices;

namespace GymPassport.GymPassportAPI.Commands
{
    public class CreateClientCommand : ICreateClientCommand
    {
        private readonly ClientApiConnector _clientApiConnector;

        public CreateClientCommand(ClientApiConnector clientApiConnector)
        {
            _clientApiConnector = clientApiConnector;
        }

        public async Task Execute(string authToken, Client newClient)
        {
            ClientService clientService = new ClientService(_clientApiConnector);
            await clientService.InsertClient(authToken, newClient);
        }
    }
}

using GymPassport.Domain.Commands;
using GymPassport.GymPassportAPI.ApiConnectors;
using GymPassport.GymPassportAPI.Services.ClientServices;

namespace GymPassport.GymPassportAPI.Commands
{
    public class DeleteClientCommand : IDeleteClientCommand
    {
        private readonly IClientApiConnector _clientApiConnector;

        public DeleteClientCommand(IClientApiConnector clientApiConnector)
        {
            _clientApiConnector = clientApiConnector;
        }

        public async Task Execute(string authToken, string username)
        {
            ClientService clientService = new ClientService(_clientApiConnector);
            await clientService.DeleteClient(authToken, username);
        }
    }
}

using GymPassport.Domain.Commands;
using GymPassport.GymPassportAPI.Services.ClientServices;

namespace GymPassport.GymPassportAPI.Commands
{
    public class DeleteClientCommand : IDeleteClientCommand
    {
        private readonly IClientService _clientService;

        public DeleteClientCommand(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task Execute(string accessToken, string username)
        {
            await _clientService.DeleteClient(accessToken, username);
        }
    }
}

using GymPassport.Domain.Commands;
using GymPassport.Domain.Models;
using GymPassport.Domain.Queries;
using GymPassport.WPF.State.Accounts;

namespace GymPassport.WPF.State.Clients
{
    public class ClientsStore
    {
        private readonly IAccountStore _accountStore;
        private readonly IGetAllClientsQuery _getAllClientsQuery;
        private readonly ICreateClientCommand _createClientCommand;
        private readonly IUpdateClientCommand _updateClientCommand;
        private readonly IDeleteClientCommand _deleteClientCommand;

        private readonly List<Client> _clients;
        public IEnumerable<Client> Clients => _clients;

        public event Action ClientsLoaded;
        public event Action<Client> ClientAdded;
        public event Action<Client> ClientUpdated;
        public event Action<string> ClientDeleted;

        public ClientsStore(IAccountStore accountStore,
            IGetAllClientsQuery getAllClientsQuery,
            ICreateClientCommand createClientCommand,
            IUpdateClientCommand updateClientCommand,
            IDeleteClientCommand deleteClientCommand)
        {
            _accountStore = accountStore;
            _getAllClientsQuery = getAllClientsQuery;
            _createClientCommand = createClientCommand;
            _updateClientCommand = updateClientCommand;
            _deleteClientCommand = deleteClientCommand;

            _clients = new List<Client>();
        }

        public async Task Load()
        {
            IEnumerable<Client> clients = await _getAllClientsQuery.Execute(_accountStore.CurrentAccount.AuthToken);

            _clients.Clear();
            _clients.AddRange(clients);

            ClientsLoaded?.Invoke();
        }

        public async Task Add(Client client)
        {
            await _createClientCommand.Execute(_accountStore.CurrentAccount.AuthToken, client);

            _clients.Add(client);

            ClientAdded?.Invoke(client);
        }

        public async Task Update(Client client)
        {
            await _updateClientCommand.Execute(_accountStore.CurrentAccount.AuthToken, client);

            int currentIndex = _clients.FindIndex(y => y.Username == client.Username);

            if (currentIndex != -1)
            {
                _clients[currentIndex] = client;
            }
            else
            {
                _clients.Add(client);
            }

            ClientUpdated?.Invoke(client);
        }

        public async Task Delete(string username)
        {
            await _deleteClientCommand.Execute(_accountStore.CurrentAccount.AuthToken, username);

            _clients.RemoveAll(y => y.Username == username);

            ClientDeleted?.Invoke(username);
        }
    }
}

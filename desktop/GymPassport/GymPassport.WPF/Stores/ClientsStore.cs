using GymPassport.WPF.ViewModels;

namespace GymPassport.WPF.Stores
{
    public class ClientsStore
    {
        public event Action<ClientViewModel> ClientAdded;
        public event Action<ClientViewModel> ClientRemoved;

        public void AddClient(ClientViewModel client)
        {
            ClientAdded?.Invoke(client);
        }

        public void RemoveClient(ClientViewModel client)
        {
            ClientRemoved?.Invoke(client);
        }
    }
}

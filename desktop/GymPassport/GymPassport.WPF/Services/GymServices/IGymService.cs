using GymPassport.WPF.Models;
using GymPassport.WPF.ViewModels;
using System.Collections.ObjectModel;

namespace GymPassport.WPF.Services.GymServices
{
    public interface IGymService
    {
        Task<ObservableCollection<ClientViewModel>> GetAllGymClients(string token);
        Task<string> UpdateGym(string accessToken, Gym gym);
    }
}

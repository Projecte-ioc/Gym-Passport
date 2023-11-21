using Gym_Passport.Commands;
using Gym_Passport.Models;
using Gym_Passport.State.Accounts;
using Gym_Passport.ViewModels;
using GymPassportPruebasAPI.Services.ClientServices;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Gym_Passport_Navigation.Commands
{
    public class AddClientCommand : AsyncCommandBase
    {
        private readonly ClientsViewModel _clientsViewModel;
        private readonly IAccountStore _accountStore;
        private readonly IClientService _clientService;

        public AddClientCommand(ClientsViewModel clientsViewModel, IAccountStore accountStore, IClientService clientService)
        {
            _clientsViewModel = clientsViewModel;
            _accountStore = accountStore;
            _clientService = clientService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            if(_clientsViewModel.CurrentClient != null)
            {
                //await _clientService.InsertClient(_accountStore.CurrentAccount.Token, new Client
                //{
                //    Name = _clientsViewModel.CurrentClient.Name,
                //    Password = _clientsViewModel.CurrentClient.Password,
                //    Role = _clientsViewModel.Roles_SelectedValue,
                //    Username = _clientsViewModel.CurrentClient.Username
                //});

                MessageBox.Show(
                    _clientsViewModel.CurrentClient.Username + "\n" +
                    _clientsViewModel.CurrentClient.Name + "\n" +
                    _clientsViewModel.Roles_SelectedValue + "\n" +
                    _clientsViewModel.CurrentClient.Password
                    );
            }
            else
            {
                MessageBox.Show("El CurrentClient es null.");
            }
        }
    }
}

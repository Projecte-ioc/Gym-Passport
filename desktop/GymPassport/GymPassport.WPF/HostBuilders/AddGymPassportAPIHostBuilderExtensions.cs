using GymPassport.Domain.Commands;
using GymPassport.Domain.Queries;
using GymPassport.GymPassportAPI.Commands;
using GymPassport.GymPassportAPI.Queries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GymPassport.WPF.HostBuilders
{
    public static class AddGymPassportAPIHostBuilderExtensions
    {
        public static IHostBuilder AddGymPassportAPI(this IHostBuilder host)
        {
            host.ConfigureServices((context, services) =>
            {
                // Db Services (Queries and commands)
                services.AddSingleton<IGetAllClientsQuery, GetAllClientsQuery>();
                services.AddSingleton<ICreateClientCommand, CreateClientCommand>();
                services.AddSingleton<IUpdateClientCommand, UpdateClientCommand>();
                services.AddSingleton<IDeleteClientCommand, DeleteClientCommand>();

                services.AddSingleton<IGetAllGymEventsQuery, GetAllGymEventsQuery>();
                services.AddSingleton<ICreateGymEventCommand, CreateGymEventCommand>();
                services.AddSingleton<IUpdateGymEventCommand, UpdateGymEventCommand>();
                services.AddSingleton<IDeleteGymEventCommand, DeleteGymEventCommand>();
            });

            return host;
        }
    }
}

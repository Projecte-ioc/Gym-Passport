using GymPassport.GymPassportAPI.Services.AuthenticationServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GymPassport.WPF.HostBuilders
{
    public static class AddConfigurationHostBuilderExtensions
    {
        public static IHostBuilder AddConfiguration(this IHostBuilder host)
        {
            host.ConfigureAppConfiguration(c =>
            {
                c.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                c.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                c.AddEnvironmentVariables();
            })
            .ConfigureServices((hostContext, services) =>
            {
                // Configuración de servicios
                services.Configure<AppSettings>(hostContext.Configuration);
            });

            return host;
        }
    }
}

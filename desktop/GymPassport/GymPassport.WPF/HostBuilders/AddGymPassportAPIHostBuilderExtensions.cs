using Microsoft.Extensions.Hosting;

namespace GymPassport.WPF.HostBuilders
{
    public static class AddGymPassportAPIHostBuilderExtensions
    {
        //public static IHostBuilder AddGymPassportAPI(this IHostBuilder host)
        //{
        //    host.ConfigureServices((context, services) =>
        //    {
        //        string apiKey = context.Configuration.GetValue<string>("FINANCE_API_KEY");
        //        services.AddSingleton(new FinancialModelingPrepAPIKey(apiKey));

        //        services.AddHttpClient<FinancialModelingPrepHttpClient>(c =>
        //        {
        //            c.BaseAddress = new Uri("https://financialmodelingprep.com/api/v3/");
        //        });
        //    });

        //    return host;
        //}
    }
}

using LegislativeApp.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LegislativeApp.Application.Configuration;

public static class RepositoriesConfiguration
{
    public static IHostBuilder AddRepositoriesConfiguration(this IHostBuilder host)
    {
        host.ConfigureServices((context, services) =>
        {
            services.AddSingleton<ICsvRepository, CsvRepository>();
            services.AddSingleton<ILegislativeRepository, LegislativeRepository>();
        });
        return host;
    }
}
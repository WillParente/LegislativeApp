using LegislativeApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LegislativeApp.Application.Configuration;

public static class ServicesConfiguration
{
    public static IHostBuilder AddServicesConfiguration(this IHostBuilder host)
    {
        host.ConfigureServices((context, services) =>
        {
            services.AddSingleton<ILegislativeService, LegislativeService>();
            services.AddSingleton<IReportService, ReportService>();
        });

        return host;
    }
}
using LegislativeApp.Application.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LegislativeApp.Application.Configuration;

internal static class AppSettingsConfiguration
{
    internal static IHostBuilder AddAppSettingsConfiguration(this IHostBuilder host)
    {
        host.ConfigureAppConfiguration(c =>
        {
            c.AddJsonFile(path: "appsettings.json", optional: true, reloadOnChange: true);
            c.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true);
            c.AddEnvironmentVariables();
        });

        host.ConfigureServices((context, services) =>
        {
            services.Configure<AppSettings>(context.Configuration.GetSection(nameof(AppSettings)));
        });

        return host;
    }
}

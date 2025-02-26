using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using LegislativeApp.Application.Configuration;
using LegislativeApp.Application.Services;

var host = Host.CreateDefaultBuilder(args)
            .AddAppSettingsConfiguration()
            .AddRepositoriesConfiguration()
            .AddServicesConfiguration()
            .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration))
            .Build();

var app = host.Services.GetRequiredService<ILegislativeService>();

await app.RunAsync(args);
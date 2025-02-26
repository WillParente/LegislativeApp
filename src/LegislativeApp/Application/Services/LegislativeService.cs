using LegislativeApp.Application.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;

namespace LegislativeApp.Application.Services;

public class LegislativeService : ILegislativeService
{
    private readonly ILogger<LegislativeService> _logger;
    private readonly IReportService _reportService;
    private readonly AppSettings _settings;

    public LegislativeService(ILogger<LegislativeService> logger, IReportService reportService, IOptions<AppSettings> settings)
    {
        _logger = logger;
        _reportService = reportService;
        _settings = settings.Value;
    }

    public async Task RunAsync(string[] args)
    {
        _logger.LogInformation($"Stating application");
        DateTime startTime = DateTime.Now;

        _logger.LogDebug("Retrieving data from appsettings");
        var filesRootPath = _settings.FilesRootPath;

        _logger.LogDebug("Running legislative reports in parallel...");
        await Task.WhenAll
            (
                _reportService.GenerateLegislatorVotesReportAsync(csvPath: filesRootPath, reportName: "legislators-support-oppose-count.csv", cancellationToken: CancellationToken.None),
                _reportService.GenerateBillsReportAsync(csvPath: filesRootPath, reportName: "bills.csv.", cancellationToken: CancellationToken.None) 
            );

        _logger.LogInformation("======================================================================");
        _logger.LogInformation($"Closing application - Total Execution Time: {DateTime.Now - startTime}");
        _logger.LogInformation("======================================================================");
    }
}
namespace LegislativeApp.Application.Services;

public interface IReportService
{
    Task GenerateLegislatorVotesReportAsync(string csvPath, string reportName, CancellationToken cancellationToken);
    Task GenerateBillsReportAsync(string csvPath, string reportName, CancellationToken cancellationToken);
}

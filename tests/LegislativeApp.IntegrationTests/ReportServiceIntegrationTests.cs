using LegislativeApp.Application.Services;
using LegislativeApp.Domain.Entities;
using LegislativeApp.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace LegislativeApp.Tests.IntegrationTests;

public class ReportServiceIntegrationTests
{
    private readonly CsvRepository _csvRepository;
    private readonly LegislativeRepository _legislativeRepository;
    private readonly ReportService _reportService;

    public ReportServiceIntegrationTests()
    {
        _csvRepository = new CsvRepository();
        _legislativeRepository = new LegislativeRepository(_csvRepository);
        var mockLogger = new Mock<ILogger<ReportService>>();
        _reportService = new ReportService(mockLogger.Object, _legislativeRepository);
    }

    [Fact]
    public async Task GenerateBillsReportAsync_ShouldGenerateReport_WhenCalled()
    {
        // Arrange
        var bills = new List<Bill>
        {
            new Bill { Id = 1, Title = "Bill1", SponsorId = 1 },
            new Bill { Id = 2, Title = "Bill2", SponsorId = 2 }
        };
        var votes = new List<Vote>
        {
            new Vote { Id = 1, BillId = 1, VoteType = VoteType.Yea },
            new Vote { Id = 2, BillId = 1, VoteType = VoteType.Nay }
        };
        var voteResults = new List<VoteResult>
        {
            new VoteResult { VoteId = 1, LegislatorId = 1, VoteType = VoteType.Yea },
            new VoteResult { VoteId = 2, LegislatorId = 2, VoteType = VoteType.Nay }
        };

        await _csvRepository.WriteRecordsAsync("bills.csv", bills, CancellationToken.None);
        await _csvRepository.WriteRecordsAsync("votes.csv", votes, CancellationToken.None);
        await _csvRepository.WriteRecordsAsync("voteResults.csv", voteResults, CancellationToken.None);

        // Act
        await _reportService.GenerateBillsReportAsync("bills.csv", "BillsReport", CancellationToken.None);

        // Assert
        // Additional assertions can be added to verify the report generation logic
    }
}

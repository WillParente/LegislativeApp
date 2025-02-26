using LegislativeApp.Application.Services;
using LegislativeApp.Domain.Entities;
using LegislativeApp.Domain.Enums;
using LegislativeApp.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace LegislativeApp.Tests;

public class ReportServiceTests
{
    private readonly Mock<ILogger<ReportService>> _mockLogger;
    private readonly Mock<ILegislativeRepository> _mockLegislativeRepository;
    private readonly ReportService _reportService;

    public ReportServiceTests()
    {
        _mockLogger = new Mock<ILogger<ReportService>>();
        _mockLegislativeRepository = new Mock<ILegislativeRepository>();
        _reportService = new ReportService(_mockLogger.Object, _mockLegislativeRepository.Object);
    }

    [Fact]
    public async Task GenerateBillsReportAsync_ShouldGenerateReport_WhenCalled()
    {
        // Arrange
        var bills = new List<Bill>
        {
            new Bill(1, "Bill1", 1),
            new Bill(2, "Bill2", 2)
        };
        var votes = new List<Vote>
        {
            new Vote(1, 1),
            new Vote(2, 1)
        };
        var voteResults = new List<VoteResult>
        {
            new VoteResult(1, 1, 1, VoteType.Yea),
            new VoteResult(2, 2, 2, VoteType.Nay)
        };

        _mockLegislativeRepository.Setup(repo => repo.LoadBillsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(bills);
        _mockLegislativeRepository.Setup(repo => repo.LoadVotesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(votes);
        _mockLegislativeRepository.Setup(repo => repo.LoadVoteResultsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(voteResults);

        var expectedReportPath = "path/to/bills.csv/Reports/BillsReport";

        // Act
        await _reportService.GenerateBillsReportAsync("path/to/bills.csv", "BillsReport", CancellationToken.None);

        // Assert
        _mockLegislativeRepository.Verify(repo => repo.ExportReportAsync(
            It.Is<string>(path => path.EndsWith("Reports/BillsReport") || path.EndsWith("Reports\\BillsReport")),
            It.IsAny<IEnumerable<object>>(),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GenerateLegislatorVotesReportAsync_ShouldGenerateReport_WhenCalled()
    {
        // Arrange
        var legislators = new List<Legislator>
        {
            new Legislator(1, "Legislator1"),
            new Legislator(2, "Legislator2")
        };
        var voteResults = new List<VoteResult>
        {
            new VoteResult(1, 1, 1, VoteType.Yea),
            new VoteResult(2, 2, 2, VoteType.Nay)
        };
        var votes = new List<Vote>
        {
            new Vote(1, 1),
            new Vote(2, 1)
        };

        _mockLegislativeRepository.Setup(repo => repo.LoadLegislatorsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(legislators);
        _mockLegislativeRepository.Setup(repo => repo.LoadVotesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(votes);
        _mockLegislativeRepository.Setup(repo => repo.LoadVoteResultsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(voteResults);

        var expectedReportPath = "path/to/legislators.csv/Reports/LegislatorVotesReport";

        // Act
        await _reportService.GenerateLegislatorVotesReportAsync("path/to/legislators.csv", "LegislatorVotesReport", CancellationToken.None);

        // Assert
        _mockLegislativeRepository.Verify(repo => repo.ExportReportAsync(
            It.Is<string>(path => path.EndsWith("Reports/LegislatorVotesReport") || path.EndsWith("Reports\\LegislatorVotesReport")),
            It.IsAny<IEnumerable<object>>(),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
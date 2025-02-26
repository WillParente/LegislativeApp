using LegislativeApp.Domain.Entities;
using LegislativeApp.Domain.Enums;
using LegislativeApp.Infrastructure.Repositories;
using Moq;

namespace LegislativeApp.Tests;

public class LegislativeRepositoryTests
{
    private readonly Mock<ICsvRepository> _mockCsvRepository;
    private readonly LegislativeRepository _legislativeRepository;

    public LegislativeRepositoryTests()
    {
        _mockCsvRepository = new Mock<ICsvRepository>();
        _legislativeRepository = new LegislativeRepository(_mockCsvRepository.Object);
    }

    [Fact]
    public async Task LoadLegislatorsAsync_ShouldReturnLegislators_WhenCalled()
    {
        // Arrange
        var legislators = new List<Legislator>
            {
                new Legislator(id: 1, name: "Legislator1"),
                new Legislator(id: 2, name: "Legislator2")
            };
        _mockCsvRepository.Setup(repo => repo.ReadRecordsAsync<Legislator>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(legislators);

        // Act
        var result = await _legislativeRepository.LoadLegislatorsAsync("path/to/legislators.csv", CancellationToken.None);

        // Assert
        Assert.Equal(legislators.Count, result.Count());
    }

    [Fact]
    public async Task LoadBillsAsync_ShouldReturnBills_WhenCalled()
    {
        // Arrange
        var bills = new List<Bill>
            {
                new Bill(id:1, title: "Bill1", sponsorId: 1),
                new Bill(id:2, title: "Bill2", sponsorId: 2)
            };
        _mockCsvRepository.Setup(repo => repo.ReadRecordsAsync<Bill>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(bills);

        // Act
        var result = await _legislativeRepository.LoadBillsAsync("path/to/bills.csv", CancellationToken.None);

        // Assert
        Assert.Equal(bills.Count, result.Count());
    }

    [Fact]
    public async Task LoadVotesAsync_ShouldReturnVotes_WhenCalled()
    {
        // Arrange
        var votes = new List<Vote>
            {
                new Vote(id: 1, billId: 1),
                new Vote(id: 2, billId: 2)
            };
        _mockCsvRepository.Setup(repo => repo.ReadRecordsAsync<Vote>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(votes);

        // Act
        var result = await _legislativeRepository.LoadVotesAsync("path/to/votes.csv", CancellationToken.None);

        // Assert
        Assert.Equal(votes.Count, result.Count());
    }

    [Fact]
    public async Task LoadVoteResultsAsync_ShouldReturnVoteResults_WhenCalled()
    {
        // Arrange
        var voteResults = new List<VoteResult>
            {
                new VoteResult(id: 1, legislatorId: 1, voteId: 1, voteType: VoteType.Yea),
                new VoteResult(id: 2, legislatorId: 2, voteId: 2, voteType: VoteType.Nay)
            };
        _mockCsvRepository.Setup(repo => repo.ReadRecordsAsync<VoteResult>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(voteResults);

        // Act
        var result = await _legislativeRepository.LoadVoteResultsAsync("path/to/voteResults.csv", CancellationToken.None);

        // Assert
        Assert.Equal(voteResults.Count, result.Count());
    }

    [Fact]
    public async Task ExportReportAsync_ShouldCallWriteRecordsAsync_WhenCalled()
    {
        // Arrange
        var legislators = new List<Legislator>
            {
                new Legislator(id: 1, name: "Legislator1"),
                new Legislator(id: 2, name: "Legislator2")
            };

        // Act
        await _legislativeRepository.ExportReportAsync("path/to/report.csv", legislators, CancellationToken.None);

        // Assert
        _mockCsvRepository.Verify(repo => repo.WriteRecordsAsync("path/to/report.csv", legislators, CancellationToken.None), Times.Once);
    }
}

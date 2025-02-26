using LegislativeApp.Domain.Entities;
using LegislativeApp.Infrastructure.Repositories;

namespace LegislativeApp.Tests.IntegrationTests;

public class LegislativeRepositoryIntegrationTests
{
    private readonly CsvRepository _csvRepository;
    private readonly LegislativeRepository _legislativeRepository;

    public LegislativeRepositoryIntegrationTests()
    {
        _csvRepository = new CsvRepository();
        _legislativeRepository = new LegislativeRepository(_csvRepository);
    }

    [Fact]
    public async Task LoadLegislatorsAsync_ShouldReturnLegislators_WhenFileExists()
    {
        // Arrange
        var path = "test_legislators.csv";
        var expectedLegislators = new List<Legislator>
        {
            new Legislator(id: 1, name: "Legislator1"),
            new Legislator(id: 2, name: "Legislator2")
        };
        await _csvRepository.WriteRecordsAsync(path, expectedLegislators, CancellationToken.None);

        // Act
        var result = await _legislativeRepository.LoadLegislatorsAsync(path, CancellationToken.None);

        // Assert
        Assert.Equal(expectedLegislators.Count, result.Count());
    }
}

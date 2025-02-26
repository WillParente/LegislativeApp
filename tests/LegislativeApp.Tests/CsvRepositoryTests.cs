using LegislativeApp.Infrastructure.Repositories;

namespace LegislativeApp.Tests;

public class CsvRepositoryTests
{
    private readonly CsvRepository _csvRepository;

    public CsvRepositoryTests()
    {
        _csvRepository = new CsvRepository();
    }

    [Fact]
    public async Task WriteRecordsAsync_ShouldCreateFile_WhenCalled()
    {
        // Arrange
        var path = Path.Combine(Directory.GetCurrentDirectory(), "output.csv"); // Caminho no diretório atual
        var records = new List<TestRecord>
        {
            new TestRecord { Id = 1, Name = "Test1" }
        };

        // Act
        await _csvRepository.WriteRecordsAsync(path, records, CancellationToken.None);

        // Assert
        Assert.True(File.Exists(path));
        File.Delete(path);
    }

    [Fact]
    public async Task ReadRecordsAsync_ShouldReturnRecords_WhenFileExists()
    {
        // Arrange
        var path = Path.Combine(Directory.GetCurrentDirectory(), "test.csv"); // Caminho no diretório atual
        var expectedRecords = new List<TestRecord>
        {
            new TestRecord { Id = 1, Name = "Test1" },
            new TestRecord { Id = 2, Name = "Test2" }
        };
        
        await _csvRepository.WriteRecordsAsync(path, expectedRecords, CancellationToken.None);

        // Act
        var records = await _csvRepository.ReadRecordsAsync<TestRecord>(path, CancellationToken.None);

        // Assert
        Assert.Equal(expectedRecords.Count, records.Count());
        File.Delete(path);
    }

    private class TestRecord
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

using LegislativeApp.Domain.Entities;

namespace LegislativeApp.Infrastructure.Repositories;

public class LegislativeRepository : ILegislativeRepository
{
    private readonly ICsvRepository _csvRepository;

    public LegislativeRepository(ICsvRepository csvRepository)
    {
        _csvRepository = csvRepository;
    }

    public async Task<IEnumerable<Legislator>> LoadLegislatorsAsync(string filePath, CancellationToken cancellationToken)
    {
        return await _csvRepository.ReadRecordsAsync<Legislator>(filePath, cancellationToken);
    }

    public async Task<IEnumerable<Bill>> LoadBillsAsync(string filePath, CancellationToken cancellationToken)
    {
        return await _csvRepository.ReadRecordsAsync<Bill>(filePath, cancellationToken);
    }

    public async Task<IEnumerable<Vote>> LoadVotesAsync(string filePath, CancellationToken cancellationToken)
    {
        return await _csvRepository.ReadRecordsAsync<Vote>(filePath, cancellationToken);
    }

    public async Task<IEnumerable<VoteResult>> LoadVoteResultsAsync(string filePath, CancellationToken cancellationToken)
    {
        return await _csvRepository.ReadRecordsAsync<VoteResult>(filePath, cancellationToken);
    }

    public async Task ExportReportAsync<T>(string filePath, IEnumerable<T> records, CancellationToken cancellationToken)
    {
        await _csvRepository.WriteRecordsAsync(filePath, records, cancellationToken);
    }
}

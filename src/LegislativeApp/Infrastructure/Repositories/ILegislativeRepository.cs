using LegislativeApp.Domain.Entities;

namespace LegislativeApp.Infrastructure.Repositories;

public interface ILegislativeRepository
{
    Task<IEnumerable<Legislator>> LoadLegislatorsAsync(string filePath, CancellationToken cancellationToken);
    Task<IEnumerable<Bill>> LoadBillsAsync(string filePath, CancellationToken cancellationToken);
    Task<IEnumerable<Vote>> LoadVotesAsync(string filePath, CancellationToken cancellationToken);
    Task<IEnumerable<VoteResult>> LoadVoteResultsAsync(string filePath, CancellationToken cancellationToken);
    Task ExportReportAsync<T>(string filePath, IEnumerable<T> records, CancellationToken cancellationToken);
}
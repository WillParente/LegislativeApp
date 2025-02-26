namespace LegislativeApp.Infrastructure.Repositories;

public interface ICsvRepository
{
    Task<IEnumerable<T>> ReadRecordsAsync<T>(string path, CancellationToken cancellationToken);
    Task WriteRecordsAsync<T>(string path, IEnumerable<T> records, CancellationToken cancellationToken);
}
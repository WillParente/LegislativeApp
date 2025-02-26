using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace LegislativeApp.Infrastructure.Repositories;

public class CsvRepository : ICsvRepository
{
    public async Task<IEnumerable<T>> ReadRecordsAsync<T>(string path, CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) 
                            { 
                                Delimiter = ",",
                                HeaderValidated = null,
                                MissingFieldFound = null
                            });

        var records = new List<T>();
        await foreach (var record in csv.GetRecordsAsync<T>(cancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            records.Add(record);
        }

        return records;
    }

    public async Task WriteRecordsAsync<T>(string path, IEnumerable<T> records, CancellationToken cancellationToken)
    {
        var directory = Path.GetDirectoryName(path);
        if (directory is not null && !Directory.Exists(path: directory))
        {
            Directory.CreateDirectory(path: directory);
        }

        using var writer = new StreamWriter(path);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        await csv.WriteRecordsAsync(records, cancellationToken);
        await writer.FlushAsync(cancellationToken);
    }
}